using System;
using GraphSynth.Representation;
using OptimizationToolbox;
using GraphSynth.Evaluation;

namespace GraphSynth
{
    class outputLocationConstraint : equality
    {
        GearEvaluator gt;
        candidate c;
        double xtarget, ytarget, ztarget, theta1t, theta2t, theta3t;
        double xP, y, z, theta1, theta2, theta3;
        double[,] stateVars;


        protected override double calc(double[] x)
        {
            gt.f.calculate(x);
            int i = gt.gearcount;
            xP = (stateVars[i, 3] - xtarget);
            y = stateVars[i, 4] - ytarget;
            z = stateVars[i, 5] - ztarget;
            theta1 = Math.Abs(stateVars[i, 6]) - Math.Abs(theta1t);
            theta2 = Math.Abs(stateVars[i, 7]) - Math.Abs(theta2t);
            theta3 = Math.Abs(stateVars[i, 8]) - Math.Abs(theta3t);
            double hVal= Math.Sqrt(xP * xP + y * y + z * z + 10*theta1*theta1 + 10*theta2*theta2 + 10*theta3*theta3);
            return hVal;
        }

        #region Constructor
        public outputLocationConstraint(double[,] stateVars, candidate c, GearEvaluator gt)
        {
            this.gt = gt;
            this.c = c;
            this.stateVars = stateVars;
            this.findDerivBy = differentiate.Central2;
            this.xtarget = gt.udg.outputLocation[0,3];
            this.ytarget = gt.udg.outputLocation[1,3];
            this.ztarget = gt.udg.outputLocation[2,3];
            this.theta1t = gt.udg.outputLocation[0, 2];
            this.theta2t = gt.udg.outputLocation[1, 2];
            this.theta3t = gt.udg.outputLocation[2, 2];

        }

        public override double deriv_wrt_xi(double[] x, int i)
        {
            if (i % 4 == 3) return base.deriv_wrt_xi(x, i);
            else return 0.0;
        }
        #endregion

        
                }
            }



