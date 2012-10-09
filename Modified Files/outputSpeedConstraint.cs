using System;
using GraphSynth.Representation;
using OptimizationToolbox;
using GraphSynth.Evaluation;

namespace GraphSynth
{
    class outputSpeedConstraint:equality
    {
        double[,] stateVars;
        GearEvaluator gt;
        double outputSpeed; 
        double currentspeed;

        protected override double calc(double[] x)
        {
            gt.f.calculate(x);
            currentspeed = stateVars[gt.gearcount-1,0];
            double perdiff = Math.Abs(currentspeed - outputSpeed) / outputSpeed;

            if (perdiff < .05)
                return 0;
            else
                return currentspeed - outputSpeed;
                
                // if statement +/- the tolerance so that it is not too hard to meet
        }

        public outputSpeedConstraint(double[,] stateVars, GearEvaluator gt)
        {
            this.gt = gt;
            this.outputSpeed = gt.udg.outputSpeed;
            this.stateVars = stateVars;
            this.findDerivBy = differentiate.Central2;
        }
    }
}