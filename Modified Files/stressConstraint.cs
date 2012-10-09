using System;
using GraphSynth.Representation;
using OptimizationToolbox;
using GraphSynth.Evaluation;

namespace GraphSynth
{
    class stressConstraint : inequality
    {
        GearEvaluator gt;
        double[,] stateVars;
        candidate c;
        double Wt, F, pd, J, Ka, Km, Kv, Ks, Kb, Ki, Ca, Cm, Cs, Cf, I, d, Cv, sfb;
        double sfc, Vt, Tp, Kx, Cb, Td, z, Cxc, Ct, Cr, Cmd, Ch;
        double Cp = 0;

        protected override double calc(double[] x)
        {
            node current = null;
            node nextcurrent = null;
            gt.f.calculate(x);
            int Qv = 9;
            double Kii = 1.42;
            double Kio = 1;
            double[,] stress = new double[x.GetLength(0) / 4, 4];
            Wt = 0;
            F = 0;
            pd = 0;
            J = .30;   //table value? 
            Km = 1.6;
            Kv = 0;
            Ka = 1;
            Ks = 1;
            Kb = 1;
            Tp = 0;
            Kx = 0;
            Ki = 0; // 1.42 idler 1 other
            Ca = 0;
            Cm = 0;
            Cs = 0;
            Cf = 0;
            I = 0;
            d = 0;
            Cv = 0;
            double Nf = 2;

            for (int i = 0; i < stress.GetLength(0); i++)
            {
                    current = c.graph.nodes[i];
                    if (i < stress.GetLength(0)-1)
                        nextcurrent = c.graph.nodes[i + 1];

                #region 1) calc bending stress
                //diameter = no.teeth/pitch /2
                if (c.graph.nodes[i].localLabels.Contains("contact") && c.graph.nodes[i-1].localLabels.Contains("contact")
                    && c.graph.nodes[i+1].localLabels.Contains("contact"))
                    Ki = Kii;
                else
                    Ki = Kio;

                Wt = stateVars[i, 1];
                pd = x[i * 4 + 1];
                F = x[i * 4 + 2];
                double diameter = x[i * 4] / x[i * 4 + 1];
                double olddiameter = 0.0;
                if (i != 0) olddiameter = x[(i - 1) * 4] / x[(i - 1) * 4 + 1];
                if (i == 0)
                    d = diameter;
                else if (diameter > olddiameter)
                    d = diameter;
                else
                    d = olddiameter;
                double gearstress = 0;
                double B = Math.Pow((12 - Qv), (2 / 3)) / 4;
                double A = 50 + 56 * (1 - B);
                //pitchline velocity
                Vt = stateVars[i, 0] * (Math.PI/60) *x[i * 4] / x[i * 4 + 1];
                Kv = Math.Pow((A / (A + Math.Sqrt(Vt))), B);
                gearstress = (Wt * pd * Ka * Km * Ks * Kb * Ki) / (F * J * Kv);
                if (stateVars[i, 9] == 2)
                {
                    gearstress = 0;
                    Kx = 1;
                    Tp = Wt * d / 2;
                    gearstress = (2 * Tp * pd * Ka * Km * Ks) / (d * F * J * Kv * Kx);
                }

                if (stateVars[i, 9] == 3)
                {
                    gearstress = 0;
                }
                if (stateVars[i, 9] == 4)
                {
                    gearstress = 0;
                    //should not be 0
                }

                stress[i, 0] = gearstress;
                #endregion

                #region 2) calc wear stress
                //
                // also put in calc's for sigma-c
                double gearwear = 0;
                Ca = Ka;
                Cm = Km;
                Cv = Kv;
                Cs = Ks;
                Cf = 1;
                Cb = .634;
                Cxc = 1;
                Ct = 1;
                Cr = 1;

                if (nextcurrent.localLabels.Contains("contact")||nextcurrent.localLabels.Contains("bevelcontact"))
                {
                    if (current.localLabels.Contains("typeA") || current.localLabels.Contains("bevelA"))
                    {
                        if (nextcurrent.localLabels.Contains("typeA") || nextcurrent.localLabels.Contains("bevelA"))
                            Cp = 200;
                        else
                            Cp = 300;
                    }
                    else
                    {
                        if (nextcurrent.localLabels.Contains("typeA") || nextcurrent.localLabels.Contains("bevelA"))
                            Cp = 300;
                        else
                            Cp = 2000;
                    }
                }
                else
                    if(Cp == 0)
                    Cp = 1000;                

                I = 0.1;
                gearwear = Cp * Math.Sqrt(Wt * Ca * Cm * Cs * Cf / (F * I * d * Cv));
                if (stateVars[i, 9] == 2)
                {
                    gearwear = 0;
                    J = .21;
                    I = .08;
                    Cb = .634;
                    Cmd = 2.4;
                    Ch = 1;
                    sfc = current.localVariables[1];

                    Td = (F * I * Cv) / (2 * Cs * Cmd * Cf * Ca * Cxc) * Math.Pow((sfc * d * .774 * Ch / (Cp * Cb * Ct * Cr)), 2);
                    if (Tp < Td)
                        z = 0.667;
                    else
                        z = 1;

                    gearwear = Cp * Cb * Math.Sqrt((2 * Td / (F * I * (Math.Pow(d, 2)))) * (Math.Pow((Tp / Td), z)) * Ca * Cm * Cs * Cf * Cxc / Cv);
                }
                if (stateVars[i, 9] == 3)
                {
                    gearwear = 0;
                }
                if (stateVars[i, 9] == 4)
                {
                    gearwear = 0;
                    //should not be 0
                }
                stress[i, 1] = gearwear;
                //
                //
                #endregion

                #region 3) calc bending fatigue strength
                sfb = current.localVariables[0];
                stress[i, 2] = sfb;
                #endregion

                #region 4) calc wear fatigue strength
                sfc = current.localVariables[1];
                stress[i, 3] = sfc;
                #endregion
            }
            double penalty = double.NegativeInfinity;
            for (int i = 0; i != stress.GetLength(0); i++)
            {
                double temp = Nf * stress[i, 0] - stress[i, 2];
                if (temp > penalty) penalty = temp;
                temp = Math.Sqrt(Nf) * stress[i, 1] - stress[i, 3];
                if (temp > penalty) penalty = temp;
            }
            return penalty / 1000;
        }



        #region Constructor
        public stressConstraint(double[,] stateVars, candidate c, GearEvaluator gt)
        {
            this.stateVars = stateVars;
            this.gt = gt;
            this.c = c;
            this.findDerivBy = differentiate.Central2;
        }
        #endregion


    }
}


