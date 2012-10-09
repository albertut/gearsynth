using System;
using System.Collections.Generic;
using GraphSynth;
using GraphSynth.Representation;

namespace GraphSynth.ParamRules
{
    public partial class ParamRules
    {
        /* here are parametric rules written as part of the ruleSet.
        * these are compiled at runtime into a .dll indicated in the
        * App.config file. */
        #region Parametric Recognition Rules
        /* Parametric recognition rules receive as input:
* 1. the left hand side of the rule (L)
* 2. the entire host graph (host)
* 3. the location of the nodes in the host that L matches to (locatedNodes).
* 4. the location of the arcs in the host that L matches to (locatedArcs). */
        #endregion


        #region Parametric Application Rules
        /* Parametric application rules receive as input:
* 1. the location designGraph indicating the nodes&arcs of host that match with L (Lmapping)
* 2. the entire host graph (host)
* 3. the location of the nodes in the host that R matches to (Rmapping).
* 4. the parameters chosen by an agent for instantiating elements of Rmapping or host (parameters). */

                
/* This is APPLY for the rule entitled: beginning rule1*/
public designGraph gearspeedbegrule1(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
{
    double gearspeedrule1 = Program.inputspeed;
    /*taking the speed from the seed*/
    
    /* setting it into the place on the new gear */
    Rmapping.nodes[1].localVariables[1] = gearspeedrule1;
    Rmapping.nodes[0].localVariables[0] = gearspeedrule1;

return host;
}

/* This is APPLY for the rule entitled: beginning rule3 */
public designGraph gearspeedbegrule3(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
{
    double gearspeedrule3 = Lmapping.nodes[1].localVariables[1];
    double radius1rule3 = Lmapping.nodes[1].localVariables[3];
    double teeth1rule3 = Lmapping.nodes[1].localVariables[2];
    /* obtaining values for old properties */
    
    Rmapping.nodes[1].localVariables[0] = 0.0; 
    Rmapping.nodes[1].localVariables[1] = gearspeedrule3;
    Rmapping.nodes[1].localVariables[2] =  teeth1rule3;
    Rmapping.nodes[1].localVariables[3] = radius1rule3;
    /* setting old gear properties*/

    Rmapping.nodes[0].localVariables[0] = gearspeedrule3;
    /* setting shaft on right properties*/
    
    double teeth2rule3 = Rmapping.nodes[2].localVariables[2];
    double radius2rule3 = Rmapping.nodes[2].localVariables[3];
    /*obtaining new gear properties from right*/
    
    Rmapping.nodes[2].localVariables[0] = -(teeth1rule3/teeth2rule3);
    Rmapping.nodes[2].localVariables[1] = -((gearspeedrule3 * radius1rule3)/radius2rule3);
    /* setting new properties with calculations*/
    /* put the mechanical advantage into the 0 slot under the gear label*/
    /*slot 0- MA, slot 1 - speed, slot 2 - teeth(catalog), slot 3 - radius(catalog)*/
return host;
}

/* This is APPLY for the rule entitled: gearshaftgear */
public designGraph gearspeedgsg(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
{
    double gearmagsg = Lmapping.nodes[0].localVariables[0];
    double gearspeed1gsg = Lmapping.nodes[0].localVariables[1];
    double radius1gsg = Lmapping.nodes[0].localVariables[3];
    double teeth1gsg = Lmapping.nodes[0].localVariables[2];
    /* obtaining values for old properties */

    Rmapping.nodes[1].localVariables[0] = gearspeed1gsg;
    /* setting shaft on right properties*/
    
    Rmapping.nodes[0].localVariables[1] = gearspeed1gsg;
    Rmapping.nodes[0].localVariables[2] = teeth1gsg;
    Rmapping.nodes[0].localVariables[3] = radius1gsg;
    Rmapping.nodes[0].localVariables[0] = gearmagsg;
    /* setting old gear properties*/
    
    double teeth2gsg = Rmapping.nodes[4].localVariables[2];
    double radius2gsg = Rmapping.nodes[4].localVariables[3];
    /*obtaining new gear properties from right*/
    
    Rmapping.nodes[4].localVariables[0] = (-(teeth1gsg/teeth2gsg)) * gearmagsg;
    Rmapping.nodes[4].localVariables[1] = -((gearspeed1gsg * radius1gsg)/radius2gsg);
    /* setting new properties with calculations*/

return host;
}

/* This is APPLY for the rules entitled: singleidler*/
public designGraph gearspeedidler(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
{
    double gearmaidle = Lmapping.nodes[0].localVariables[0];
    double gearspeedidle = Lmapping.nodes[0].localVariables[1];
    double radius1idle = Lmapping.nodes[0].localVariables[3];
    double teeth1idle = Lmapping.nodes[0].localVariables[2];
    /* obtaining values for old properties */
    
    /*Shaft speed is already set to 0*/

    Rmapping.nodes[0].localVariables[1] = gearspeedidle;
    Rmapping.nodes[0].localVariables[2] = teeth1idle;
    Rmapping.nodes[0].localVariables[3] = radius1idle;
    Rmapping.nodes[0].localVariables[0] = gearmaidle;
    /* setting old gear properties*/
    
    double teeth2idle = Rmapping.nodes[3].localVariables[2];
    double radius2idle = Rmapping.nodes[3].localVariables[3];
    /*obtaining new gear properties from right*/
    
    Rmapping.nodes[3].localVariables[0] = (-(teeth1idle/teeth2idle)) * gearmaidle;
    Rmapping.nodes[3].localVariables[1] = -((gearspeedidle * radius1idle)/radius2idle);
    /* setting new properties with calculations*/
    
return host;
}

/* This is APPLY for the rules entitled: smallergearonshaft*/
public designGraph gearspeedsgs(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
{
    double gearmasgs = Lmapping.nodes[0].localVariables[0];
    double gearspeed1 = Lmapping.nodes[0].localVariables[1];
    double radius1sgs = Lmapping.nodes[0].localVariables[3];
    double teeth1sgs = Lmapping.nodes[0].localVariables[2];
    /* obtaining values for old gear 1 properties */

    double gearma2sgs = Lmapping.nodes[1].localVariables[0];
    double gearspeed2 = Lmapping.nodes[1].localVariables[1];
    double radius2sgs = Lmapping.nodes[1].localVariables[3];
    double teeth2sgs = Lmapping.nodes[1].localVariables[2];
    /* obtaining values for old gear 2 properties */

    Rmapping.nodes[1].localVariables[0] = gearspeed1;
    /* setting shaft on right gear 2 properties*/

    Rmapping.nodes[0].localVariables[1] = gearspeed1;
    Rmapping.nodes[0].localVariables[2] = teeth1sgs;
    Rmapping.nodes[0].localVariables[3] = radius1sgs;
    Rmapping.nodes[0].localVariables[0] = gearmasgs;
    /* setting old gear 1 properties*/

    Rmapping.nodes[5].localVariables[1] = gearspeed2;
    Rmapping.nodes[5].localVariables[2] = teeth2sgs;
    Rmapping.nodes[5].localVariables[3] = radius2sgs;
    Rmapping.nodes[5].localVariables[0] = gearma2sgs;
    /* setting old gear 2 properties*/

    double teeth3sgs = Rmapping.nodes[4].localVariables[2];
    double radius3sgs = Rmapping.nodes[4].localVariables[3];
    /*obtaining new gear properties from right*/

    Rmapping.nodes[4].localVariables[0] = (-(teeth1sgs/teeth3sgs))*gearmasgs;
    Rmapping.nodes[4].localVariables[1] = gearspeed1;
    /* setting new properties with calculations*/
    
return host;
}

/* This is APPLY for the rule entitled: aftersgos */
public designGraph gearspeedas(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
{
    double gearmaas = Lmapping.nodes[0].localVariables[0];
    double gearspeedas = Lmapping.nodes[0].localVariables[1];
    double radius1as = Lmapping.nodes[0].localVariables[3];
    double teeth1as = Lmapping.nodes[0].localVariables[2];
    /* obtaining values for old properties */

    Rmapping.nodes[0].localVariables[1] = gearspeedas;
    Rmapping.nodes[0].localVariables[2] = teeth1as;
    Rmapping.nodes[0].localVariables[3] = radius1as;
    Rmapping.nodes[0].localVariables[0] = gearmaas;
    /* setting old gear properties*/

    double teeth2as = Rmapping.nodes[1].localVariables[2];
    double radius2as = Rmapping.nodes[1].localVariables[3];
    /*obtaining new gear properties from right*/

    Rmapping.nodes[1].localVariables[0] = (-(teeth1as / teeth2as)) * gearmaas;
    Rmapping.nodes[1].localVariables[1] = -((gearspeedas * radius1as) / radius2as);
    
    return host;
}

/* This is APPLY for the rule entitled: terminal rule */
public designGraph gearspeedterm(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
{
    double gearmaterm = Lmapping.nodes[0].localVariables[0];
    double gearspeedterm = Lmapping.nodes[0].localVariables[1];
    double radius1term = Lmapping.nodes[0].localVariables[3];
    double teeth1term = Lmapping.nodes[0].localVariables[2];
    
    double gearma2term = Lmapping.nodes[1].localVariables[0];
    double gearspeed2term = Lmapping.nodes[1].localVariables[1];
    double radius2term = Lmapping.nodes[1].localVariables[3];
    double teeth2term = Lmapping.nodes[1].localVariables[2];
    /* obtaining values for old properties */

    Rmapping.nodes[1].localVariables[1] = gearspeedterm;
    Rmapping.nodes[1].localVariables[0] = gearmaterm;
    /* setting shaft on right properties*/

    Rmapping.nodes[0].localVariables[1] = gearspeedterm;
    Rmapping.nodes[0].localVariables[2] = teeth1term;
    Rmapping.nodes[0].localVariables[3] = radius1term;
    Rmapping.nodes[0].localVariables[0] = gearmaterm;
    
    Rmapping.nodes[4].localVariables[1] = gearspeed2term;
    Rmapping.nodes[4].localVariables[2] = teeth2term;
    Rmapping.nodes[4].localVariables[3] = radius2term;
    Rmapping.nodes[4].localVariables[0] = gearma2term;
    /* setting old gear properties*/

return host;
}
#endregion


    }
}
