using System;
using System.Collections.Generic;
using System.Text;
using GraphSynth.Representation;
using OptimizationToolbox;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Netron.GraphLib;


namespace GraphSynth.Evaluation
{
    public class GearEvaluator
    {

        public userDefinedGoals udg;
        private int i = 0;
        public int gearcount = 0;
        public int bearingcount = 0;
        //penalty functions
        public double p1;
        public double p2;
        public double p3;
        double[,] stateVars;
        public totalObjFunction f;
        List<GearFamily> gearFamilies;

        candidate current;

        // constructor
        public GearEvaluator(userDefinedGoals udg)
        {
            this.udg = udg;
            gearFamilies = GearFamily.loadDefaults();

        }

        public void evalGT(candidate c)
        {
            current = c;
            reorderNodes(c);
            Boolean found = false;

            /* recall that gearcount is found in reorderNodes, Albert! */
            stateVars = new double[gearcount + 1, 10];

            #region Set up optMethod
            //NelderMead optMethod =
            //    new NelderMead(.001, 10, true);
            //GradientBasedUnconstrained optMethod =
            //    new GradientBasedUnconstrained(10);
            GradientBasedOptimization optMethod = new GradientBasedOptimization(10);
            //SequentialQuadraticProgramming optMethod = new SequentialQuadraticProgramming(true);
            //GeneralizedReducedGradientActiveSet optMethod =
            //    new GeneralizedReducedGradientActiveSet(true);
            optMethod.Add(new ArithmeticMean(optMethod, 0.001, 2, 200));
            //optMethod.Add(new GoldenSection(optMethod, 0.001,200, int.MaxValue));
            //optMethod.Add(new BFGSDirection());
            optMethod.Add(new FletcherReevesDirection());
            optMethod.Add(new convergenceBasic(BasicConvergenceTypes.OrBetweenSetConditions, 200, 0.0001, double.NaN, double.NaN, int.MaxValue));
            //optMethod.Add(new convergenceBasic(BasicConvergenceTypes.AndBetweenSetConditions, 20, 0.01, double.NaN, double.NaN, int.MaxValue));
            optMethod.Add(new squaredExteriorPenalty(optMethod, 10.0));
            //optMethod.Add(new linearExteriorPenaltySum(optMethod, 10.0));
            DiscreteSpaceDescriptor dsd = new DiscreteSpaceDescriptor(optMethod, 4 * gearcount);
            optMethod.Add(dsd);
            #endregion

            for (int i = 0; i < gearcount; i++)
            {
                foreach (GearFamily gf in gearFamilies)
                    if (c.graph.nodes[i].localLabels.Contains(gf.label))
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (c.graph.nodes[i].localVariables.Count < j + 1)
                                c.graph.nodes[i].localVariables.Add(double.NaN);
                        }
                        c.graph.nodes[i].localVariables[0] = gf.Sfb;
                        c.graph.nodes[i].localVariables[1] = gf.Sfc;
                        c.graph.nodes[i].localVariables[2] = gf.density;
                        dsd.addLinkedVariableValues(new int[] { 4 * i, 4 * i + 1, 4 * i + 2 }, gf.gears);
                    }
            }
            SearchIO.output("The parametric space is " + dsd.SizeOfSpace.ToString(), 3);
            //setup constraints for optimization
            //slot 1 - number of teeth
            //slot 2 - pitch
            //slot 3 - face Width
            //slot 4 - location variable

            #region Constraint Building Region
            double[] x = new double[4 * gearcount];
            double[] xStar = new double[4 * gearcount];
            //double fStar = double.PositiveInfinity;
            double fStar = 0.0;
            double ftemp = 1000;
            double weightstar = 100;
            double lowestmass = 100;
            double massstar = 100;
            double mass = 100;
            double[,] stateVarsStar = new double[gearcount + 1, 10];

            outputSpeedConstraint oSC =
            new outputSpeedConstraint(stateVars, this);
            optMethod.Add(oSC);


            stressConstraint sc =
            new stressConstraint(stateVars, c, this);
            optMethod.Add(sc);

            boundingboxConstraint bbc =
            new boundingboxConstraint(stateVars, c, this);
            optMethod.Add(bbc);

            outputLocationConstraint olc =
            new outputLocationConstraint(stateVars, c, this);
            optMethod.Add(olc);


            List<samePitch> samePitches = new List<samePitch>();
            for (int i = 0; i < gearcount; i++)
            {
                if ((c.graph.nodes[i].localLabels.Contains("contact")) || (c.graph.nodes[i].localLabels.Contains("bevelcontact")) || (c.graph.nodes[i].localLabels.Contains("wormcontact")))
                {
                    samePitches.Add(new samePitch(((i - 1) * 4) + 1, ((i * 4) + 1)));
                }
            }
            f = new totalObjFunction(this, current, stateVars);
            optMethod.Add(f);
            #endregion
            int numVars = dsd.linkedSpace.Count;
            int[] VarIndices = new int[numVars];
            for (int i = 0; i < numVars; i++)
                VarIndices[i] = 0;
            int[] VarMaxes = new int[numVars];
            for (int i = 0; i < numVars; i++)
                VarMaxes[i] = dsd.linkedSpace[i].GetLength(0);
            int currentI = 0;
            VarIndices[currentI]--;  //this is an unavoidable hack to start at all zeroes.
            do
            {
                VarIndices[currentI]++;
                if (VarIndices[currentI] == VarMaxes[currentI])
                {
                    VarIndices[currentI] = 0;
                    currentI++;
                    if ((currentI > numVars - 3) && (currentI < numVars))
                        SearchIO.output("Index " + currentI.ToString() + " changed to " + VarIndices[currentI].ToString(), 4);
                }
                else
                {
                    currentI = 0;
                    x = dsd.GetDesignVector(null, VarIndices);
                    //fStar = f.calculate(x);
                    Boolean Feasible = true;
                    foreach (samePitch sp in samePitches)
                        if (!sp.feasible(x)) Feasible = false;
                     if (Feasible && oSC.feasible(x))
                    {
                        if (sc.feasible(x))
                        {
                            //run optMethod here
                            double[] xTuned; 
                            mass = 0;
                            double fTuned = optMethod.run(x, out xTuned);
                            for (int k = 0; k < gearcount; k++)
                            {
                                mass += stateVars[k, 2];

                            }
                            found = false;
                            if (fTuned< ftemp)
                            {
                                ftemp = fTuned;
                            }
                            found = isCurrentTheGoal(stateVars, udg);
                            if (found == true)
                            {
                                if (mass < massstar)
                                    {
                                        fStar = fTuned;
                                        xStar = (double[])xTuned.Clone();
                                        massstar = mass;
                                        stateVarsStar = (double[,])stateVars.Clone();
                                    }
                            }

                            
                        }
                    }
                }

            } while (currentI < numVars);

            SearchIO.output("final report f = " + c.f0, 3);
            if (gearcount > 2)
            {
                if (massstar == 100)
                fStar = ftemp+50;
            }
            c.f0 = fStar;
            c.f2 = massstar;
            c.f1 = calcInefficiency(stateVarsStar);
            string outputString = "";
            string outputString2 = "";
            double p = 0;
            for (int i = 0; i < gearcount; i++)
            //var order
            //x, y, z, vx, vy, vz, face width, diameter, number of teeth, type ID number
            {//output for gear visualizer!
                outputString += "gear," + (i + 1) + "," + stateVarsStar[i, 3] + "," + stateVarsStar[i, 4] +
                    "," + stateVarsStar[i, 5] + "," + stateVarsStar[i, 6] + "," + stateVarsStar[i, 7] + "," +
                    stateVarsStar[i, 8] + "," + xStar[i * 4 + 2] + "," + (xStar[i * 4] / xStar[i * 4 + 1]) +
                    "," + xStar[i * 4] + "," + stateVarsStar[i, 9] + "\n";
                if (i != 0)
                {
                    if ((stateVarsStar[i, 3] - stateVarsStar[(i - 1), 3] == 0) && (stateVarsStar[i, 4] - stateVarsStar[(i - 1), 4] == 0))
                    {
                        outputString2 += "rod," + (p) + "," + i + "," + (i - 1);//(stateVarsStar[i, 5] - stateVarsStar[(i - 1), 5]);
                        p += 1;
                    }
                }

                for (int j = 1; j < 9; j++)
                {
                    if (c.graph.nodes[i].localVariables.Count <= j)
                        c.graph.nodes[i].localVariables.Add(double.NaN);
                }
                c.graph.nodes[i].localVariables[3] = stateVarsStar[i, 3];
                c.graph.nodes[i].localVariables[4] = stateVarsStar[i, 4];
                c.graph.nodes[i].localVariables[5] = stateVarsStar[i, 5];
                c.graph.nodes[i].localVariables[6] = stateVarsStar[i, 6];
                c.graph.nodes[i].localVariables[7] = stateVarsStar[i, 7];
                c.graph.nodes[i].localVariables[8] = stateVarsStar[i, 8];
            }
            for (int j = 1; j < 9; j++)
            {
                if (c.graph.nodes[gearcount].localVariables.Count <= j)
                    c.graph.nodes[gearcount].localVariables.Add(double.NaN);
            }
            c.graph.nodes[gearcount].localVariables[3] = stateVarsStar[gearcount, 3];
            c.graph.nodes[gearcount].localVariables[4] = stateVarsStar[gearcount, 4];
            c.graph.nodes[gearcount].localVariables[5] = stateVarsStar[gearcount, 5];
            c.graph.nodes[gearcount].localVariables[6] = stateVarsStar[gearcount, 6];
            c.graph.nodes[gearcount].localVariables[7] = stateVarsStar[gearcount, 7];
            c.graph.nodes[gearcount].localVariables[8] = stateVarsStar[gearcount, 8];

            string filename = Program.settings.outputDirectory + "visualizerOutput1.txt";
            FileStream fs = new FileStream(filename, FileMode.Create);
            StreamWriter outputWriter = new StreamWriter(fs);
            outputWriter.WriteLine(outputString);
            outputWriter.WriteLine(outputString2);
            outputWriter.Close();
            fs.Close();
        }


        private void reorderNodes(candidate c)
        {
            node inputShaft = null;
            node temp;
            /* find first gear (connected to input shaft) */
            foreach (node n in c.graph.nodes)
                if (n.localLabels.Contains("seed") && n.localLabels.Contains("shaft"))
                {
                    inputShaft = n;
                    break;
                }
            foreach (arc a in inputShaft.arcs)
                if (a.otherNode(inputShaft).localLabels.Contains("gear"))
                {
                    temp = a.otherNode(inputShaft);
                    c.graph.nodes.Remove(temp);
                    c.graph.nodes.Insert(0, temp);
                    break;
                }
            gearcount = 1;
            Boolean foundNextGear;
            do
            {
                foundNextGear = false;
                node gear = c.graph.nodes[gearcount - 1];
                foreach (arc a in gear.arcsFrom)
                    if ((a.otherNode(gear).localLabels.Contains("gear")) || (a.otherNode(gear).localLabels.Contains("gear1")))
                    {
                        temp = a.otherNode(gear);
                        c.graph.nodes.Remove(temp);
                        c.graph.nodes.Insert(gearcount++, temp);
                        foundNextGear = true;
                        break;
                    }
                if (!foundNextGear)
                {
                    // this means that either that was the last gear or we need to traverse thru an idler shaft
                    foreach (arc a in gear.arcsFrom)
                        if (a.otherNode(gear).localLabels.Contains("shaft"))
                        {
                            node shaft = a.otherNode(gear);
                            foreach (arc aa in shaft.arcsFrom)
                                if ((aa.otherNode(shaft).localLabels.Contains("gear")) || (aa.otherNode(shaft).localLabels.Contains("gear1")))
                                {
                                    temp = aa.otherNode(shaft);
                                    c.graph.nodes.Remove(temp);
                                    c.graph.nodes.Insert(gearcount++, temp);
                                    foundNextGear = true;
                                    break;
                                }
                        }
                }

            } while (foundNextGear);

        }

        private double calcInefficiency(double[,] stateVarsStar)
        {
            double totEff = 100;
            int v;
            //statevars9 = gear type 1-spur, 2 - bevel, 3 - worm, 4 - worm spur
            for (v = 0; v < gearcount; v++)
            {
                if (stateVarsStar[v, 9] == 1)
                    totEff = totEff * .98;
                if (stateVarsStar[v, 9] == 2)
                    totEff = totEff * .9;
                if (stateVarsStar[v, 9] == 3)
                    totEff = totEff * 1;
                if (stateVarsStar[v, 9] == 4)
                    totEff = totEff * .7;
            }
            //100 * (1 - (Math.Pow(0.99, gearcount)));

            return (1 / totEff);
        }

        private static bool isCurrentTheGoal(double[,] stateVars, userDefinedGoals udg)
        {
            double xP, y, z, theta1, theta2, theta3;
            int gearcount = stateVars.GetLength(0)-1;
            xP = stateVars[gearcount, 3] - udg.outputLocation[0, 3];
            y = stateVars[gearcount, 4] - udg.outputLocation[1, 3];
            z = stateVars[gearcount, 5] - udg.outputLocation[2, 3];
            theta1 = Math.Abs(stateVars[gearcount, 6]) - Math.Abs(udg.outputLocation[0, 2]);
            theta2 = Math.Abs(stateVars[gearcount, 7]) - Math.Abs(udg.outputLocation[1, 2]);
            theta3 = Math.Abs(stateVars[gearcount, 8]) - Math.Abs(udg.outputLocation[2, 2]);


            double hVal = Math.Sqrt(xP * xP + y * y + z * z + theta1 * theta1 + theta2 * theta2 + theta3 * theta3);

            if (hVal < .05)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

