using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Threading;
using System.ComponentModel;
using GraphSynth.Representation;
using GraphSynth.Evaluation;
using GraphSynth.Generation;
using GraphSynth.Guidance;
using GraphSynth.Forms;
using System.Drawing.Printing;

namespace GraphSynth
{
    public static partial class Program
    {


        #region Search Process

        public static void runSearchProcess()
        {
            userDefinedGoals udg = new userDefinedGoals();
            Form2 inputBox = new Form2(udg);
            //inputBox.ShowDialog();

            ruleSet.loadAndCompileSourceFiles(rulesets, Program.settings.recompileRules, Program.settings.compiledparamRules, Program.settings.execDir);

            List<candidate> candidates = new List<candidate>();
            candidate current = null;
            candidate seedCandidate = new candidate(seed, Program.settings.numOfRuleSets);
            Boolean found = false;
            candidates.Add(seedCandidate);
            GearEvaluator ge = new GearEvaluator(udg);
            Guidance.PNormProportionalPopulationSelection GuidanceApproach
                = new Guidance.PNormProportionalPopulationSelection(0, Guidance.optimize.minimize, true, true, 1);
            int maxPop = 10000;
            while (!found && (candidates.Count != 0))
            {
                current = candidates[0];
                candidates.RemoveAt(0);
                SearchIO.iteration = current.recipe.Count;
                //RECOGNIZE
                List<option> ruleChoices = rulesets[0].recognize(current.graph);
                SearchIO.miscObject = candidates.Count;
                //
                if (current.recipe.Count >= 2)
                {
                    found = isCurrentTheGoal(current, udg);
                    if (found == true)
                    {
                        ge.evalGT(current);
                        break;
                    }
                }
                //else current.f0 = double.PositiveInfinity;

                for (int i = 0; i != ruleChoices.Count; i++)
                {
                    candidate child = current.copy();

                    ruleChoices = rulesets[0].recognize(child.graph);
                    ruleChoices[i].apply(child.graph, null);

                    child.addToRecipe(ruleChoices[i]);
                    child.f0 = double.NaN;
                    child.f1 = double.NaN;
                    child.f2 = double.NaN;
                    ge.evalGT(child);
                    child.f3 = (child.f0)/100 + child.f1;
                    //1 efficiency
                    //2 mass
                    //3 combination
                    addChildToSortedCandList(candidates, child, 3);
                    //candidates.Add(child);
                }

            }
            Program.addAndShowGraphDisplay(current.graph, "Here is your gear train!!!");
            candidate.saveToXml(current, "testTuned", settings.outputDirectory);
        }

        private static void addChildToSortedCandList(List<candidate> candidates, candidate child, int fIndex)
        {
            if (candidates.Count == 0) candidates.Add(child);
            else
            {
                int i = 0;
                double fChild = child.performanceParams[fIndex];
                while ((i < candidates.Count) && (fChild >= candidates[i].performanceParams[fIndex])) i++;
                candidates.Insert(i, child);
            }
        }

        private static bool isCurrentTheGoal(candidate current, userDefinedGoals udg)
        {
            double xP, y, z, theta1, theta2, theta3;
            int gearcount = 0;
            foreach (node n in current.graph.nodes)
            {
                if ((n.localLabels.Contains("gear")) || (n.localLabels.Contains("gear1")))
                {
                    gearcount = gearcount + 1;
                }
            }
            int i = gearcount;
            xP = current.graph.nodes[gearcount].localVariables[3] - udg.outputLocation[0, 3];
            y = current.graph.nodes[gearcount].localVariables[4] - udg.outputLocation[1, 3];
            z = current.graph.nodes[gearcount].localVariables[5] - udg.outputLocation[2, 3];
            theta1 = Math.Abs(current.graph.nodes[gearcount].localVariables[6]) - Math.Abs(udg.outputLocation[0, 2]);
            theta2 = Math.Abs(current.graph.nodes[gearcount].localVariables[7]) - Math.Abs(udg.outputLocation[1, 2]);
            theta3 = Math.Abs(current.graph.nodes[gearcount].localVariables[8]) - Math.Abs(udg.outputLocation[2, 2]);


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




        #endregion
    }
}