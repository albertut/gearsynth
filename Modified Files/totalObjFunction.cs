using System;
using System.Collections.Generic;
using OptimizationToolbox;
using GraphSynth.Representation;

namespace GraphSynth.Evaluation
{
    public class totalObjFunction : objectiveFunction
    {
        GearEvaluator gt;
        candidate c;
        double[,] stateVars;
        public totalObjFunction(GearEvaluator gt, candidate c, double[,] stateVars)
        {
            this.gt = gt;
            this.c = c;
            this.stateVars = stateVars;
            findDerivBy = differentiate.Central2;
        }
        protected override double calc(double[] x)
        {
            double totMass = 0.0;
            double currentMass = 0.0;
            double sfb, sfc, density;
            double currentspeed = gt.udg.inputSpeed;
            int gearcount = gt.gearcount;
            double inputTorque;
            double[][,] gearLocs = new double[gearcount + 1][,];

            for (int i = 0; i < gt.gearcount; i++)
            {
                double diameter = x[i * 4] / x[i * 4 + 1];
                double diameterprev = 0.0;
                if (i != 0)
                {
                    if (stateVars[i - 1 , 9] ==3 )
                    {
                        diameterprev = x[(i - 1) * 4];
                    }
                    else
                    {
                        diameterprev = x[(i - 1) * 4] / x[(i - 1) * 4 + 1];
                    }
                }
                node gear = c.graph.nodes[i];

                if (gt.udg.inputTorque == 0) inputTorque = 100;
                else inputTorque = gt.udg.inputTorque;
                if (i == 0)
                {
                    if (gear.localLabels.Contains("worm"))
                    {
                        diameter = x[i * 4];
                    }
                    stateVars[i, 1] = gt.udg.inputTorque * 12 / (diameter / 2);//new units inch/lbs
                    stateVars[i, 0] = gt.udg.inputSpeed;
                    currentspeed = gt.udg.inputSpeed;
                    gearLocs[i] = gt.udg.inputLocation;
                    stateVars[i, 9] = 1;
                    serializeCTintoStateVars(stateVars, gearLocs[i], i);
                }
                if (gear.localLabels.Contains("contact"))
                {
                    //num teeth / num teeth next gear
                    //stateVars0 = speed
                    //stateVars1 = force
                    //stateVars2 = mass
                    //stateVars3 = xlocation
                    //stateVars4 = ylocation
                    //stateVars5 = zlocation
                    //stateVars6 = angle 1
                    //stateVars7 = angle 2
                    //statevars8 = angle 3
                    //statevars9 = gear type 1-spur, 2 - bevel, 3 - worm, 4 - worm spur
                    stateVars[i, 9] = 1;
                    currentspeed = currentspeed * x[(i - 1) * 4] / x[i * 4];
                    stateVars[i, 0] = currentspeed;
                    stateVars[i, 1] = stateVars[i - 1, 1];
                    double lvar = x[(i * 4) + 3];

                    gearLocs[i] = Matrix.multiply(gearLocs[i-1], Matrix.rotationZ(lvar));
                    gearLocs[i] = Matrix.multiply(gearLocs[i],Matrix.translate((diameterprev / 2 + diameter / 2), 0.0, 0.0));
                    
                    
                    serializeCTintoStateVars(stateVars, gearLocs[i], i);
                }
                if (gear.localLabels.Contains("bevelcontact"))
                {
                    currentspeed = currentspeed * x[(i - 1) * 4] / x[i * 4];
                    stateVars[i, 0] = currentspeed;
                    stateVars[i, 9] = 2;
                    stateVars[i, 1] = stateVars[i - 1, 1];
                    double lvar = x[(i * 4) + 3];
                    gearLocs[i] = Matrix.multiply(gearLocs[i-1], Matrix.rotationY(-90));
                    gearLocs[i] = Matrix.multiply(gearLocs[i], Matrix.rotationX(lvar));
                    gearLocs[i] = Matrix.multiply(gearLocs[i], Matrix.translate((diameter/ 2), 0.0, (-diameterprev / 2)));
                    serializeCTintoStateVars(stateVars, gearLocs[i], i);
                    //SearchIO.output("i = " + i.ToString());
                }
                if (gear.localLabels.Contains("wormcontact"))
                {
                    currentspeed = currentspeed / x[i * 4];
                    stateVars[i, 9] = 4;
                    stateVars[i, 0] = currentspeed;
                    stateVars[i, 1] = stateVars[i - 1, 1];
                    double lvar = x[(i * 4) + 3];
                    gearLocs[i] = Matrix.multiply(gearLocs[i-1], Matrix.rotationY(-90));
                    gearLocs[i] = Matrix.multiply(gearLocs[i] ,Matrix.rotationZ(90));
                    gearLocs[i] = Matrix.multiply(gearLocs[i], Matrix.rotationY(lvar));
                    gearLocs[i] = Matrix.multiply(gearLocs[i], Matrix.translate(((diameterprev / 2)+ (diameter/2)), 0.0, 0.0));
                    serializeCTintoStateVars(stateVars, gearLocs[i], i);

                }

                if ((gear.localLabels.Contains("common")) || (gear.localLabels.Contains("bevelcommon")))
                {
                    stateVars[i, 0] = currentspeed;
                    stateVars[i, 1] = stateVars[i - 1, 1] * (diameterprev / 2) / (diameter / 2);
                    double lvar = x[(i * 4) + 3];
                    gearLocs[i] = Matrix.multiply(gearLocs[i - 1], Matrix.translate(0.0, 0.0, lvar));
                    serializeCTintoStateVars(stateVars, gearLocs[i], i);
                    if (gear.localLabels.Contains("common"))
                    { stateVars[i, 9] = 1; }
                    else
                    { stateVars[i, 9] = 2; }
                }
                if (gear.localLabels.Contains("worm"))
                {
                    stateVars[i, 0] = currentspeed;
                    stateVars[i, 9] = 3;
                    diameter = x[i * 4];
                    
                    if (i == 0)
                    { stateVars[i, 1] = inputTorque; }
                    else
                    {
                        double lvar = x[(i * 4) + 3];
                        stateVars[i, 1] = stateVars[i - 1, 1] * (diameterprev / 2) / (diameter / 2);
                        gearLocs[i] = Matrix.multiply(gearLocs[i - 1], Matrix.translate(0.0, 0.0, lvar));
                        serializeCTintoStateVars(stateVars, gearLocs[i], i);
                    }
                }
                if (i == (gearcount - 1) && (i != 0))
                {
                    double lvar = x[3];
                    gearLocs[i + 1] = Matrix.multiply(gearLocs[i],Matrix.translate(0.0, 0.0, lvar));
                    stateVars[i + 1, 0] = currentspeed;
                    stateVars[i + 1, 1] = 0;
                    stateVars[i + 1, 2] = 0;
                    stateVars[i + 1, 9] = 0;

                    serializeCTintoStateVars(stateVars, gearLocs[i + 1], i + 1);
                }
                density = gear.localVariables[2];
                currentMass = Math.Pow((Math.PI * (diameter / 2)), 2) * x[i * 4 + 2] * density;
                stateVars[i, 2] = currentMass;
                sfb = gear.localVariables[0];
                sfc = gear.localVariables[1];
                totMass = totMass + currentMass;
            }
            if (gt.udg.inputTorque == 0)
            {
                int j = gearcount;
                double diameter = x[j * 4] / x[j * 4 + 1];
                double diameterprev = 0.0;
                if (j != 0) diameterprev = x[(j - 1) * 4] / x[(j - 1) * 4 + 1];
                node gear = c.graph.nodes[j];
                if (j == gearcount)
                {
                    stateVars[gearcount, 1] = gt.udg.outputTorque * diameter;
                    j = j - 1;
                }
                else
                {
                    if (gear.localLabels.Contains("contact"))
                    {
                        stateVars[gearcount, 1] = stateVars[(gearcount + 1), 1];
                        j = j - 1;
                    }

                    else
                    {
                        stateVars[j, 1] = stateVars[(j - 1), 1] * (diameterprev / 2) / (diameter / 2);
                        j = j - 1;
                    }
                }
            }

            return totMass;
        }

        private void serializeCTintoStateVars(double[,] stateVars, double[,] coordTransform, int i)
        {
            stateVars[i, 3] = coordTransform[0, 3];
            stateVars[i, 4] = coordTransform[1, 3];
            stateVars[i, 5] = coordTransform[2, 3];
            stateVars[i, 6] = coordTransform[0, 2];
            stateVars[i, 7] = coordTransform[1, 2];
            stateVars[i, 8] = coordTransform[2, 2];
        }


        public override double deriv_wrt_xi(double[] x, int i)
        {
            if (i % 4 == 3) return base.deriv_wrt_xi(x, i);
            else return 0.0;
        }

    }
}