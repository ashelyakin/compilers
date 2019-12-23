using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class CountCyclesOpVisitor : AutoVisitor
    {
        int currCyc = 0;
        int opCnt = 0;
        int cycCnt = 0;

        public int MidCount()
        {
            if (cycCnt!=0)
            {
                return opCnt / cycCnt;
            }
            else
            {
                return 0;
            }
        }


        

        public override void VisitWriteNode(WriteNode w)
        {
            if (currCyc > 0)
                opCnt++;
            base.VisitWriteNode(w);
        }

        public override void VisitAssignNode(AssignNode a)
        {
            if (currCyc > 0)
                opCnt++;
            base.VisitAssignNode(a);
        }

        public override void VisitVarDefNode(VarDefNode w)
        {
            if (currCyc > 0)
                opCnt++;
            base.VisitVarDefNode(w);
        }

        public override void VisitCycleNode(CycleNode c)
        {
            if (currCyc > 0)
                opCnt++;
            cycCnt++;
            currCyc++;
            base.VisitCycleNode(c);
            currCyc--;
        }
    }
}
