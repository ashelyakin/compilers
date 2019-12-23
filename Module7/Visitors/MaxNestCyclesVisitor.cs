using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class MaxNestCyclesVisitor : AutoVisitor
    {
        public int MaxNest = 0;
        public int curDepth = 0;

        public override void VisitCycleNode(CycleNode c)
        {
            curDepth++;
            base.VisitCycleNode(c);
            if (curDepth > MaxNest)
                MaxNest = curDepth;
            curDepth--;
        }
    }

    
}
