using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class CommonlyUsedVarVisitor : AutoVisitor
    {
        Dictionary<string, int> vars = new Dictionary<string, int>();

        public string mostCommonlyUsedVar()
        {
            var max = 0;
            var str = "";
            foreach (var v in vars)
                if (v.Value > max)
                {
                    max = v.Value;
                    str = v.Key;
                }
            return str;
        }

        public override void VisitCycleNode(CycleNode c)
        {
            base.VisitCycleNode(c);
        }

        public override void VisitAssignNode(AssignNode a)
        {
            if (vars.ContainsKey(a.Id.Name))
                vars[a.Id.Name]++;
            else
                vars.Add(a.Id.Name, 0);
            base.VisitAssignNode(a);
        }

        public override void VisitWriteNode(WriteNode w)
        {
            base.VisitWriteNode(w);
        }

        public override void VisitVarDefNode(VarDefNode w)
        {
            foreach (var v in w.vars)
            {
                if (vars.ContainsKey(v.Name))
                    vars[v.Name]++;
                else
                    vars.Add(v.Name, 0);
            }
            base.VisitVarDefNode(w);
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
        }

        public override void VisitIdNode(IdNode id)
        {
            if (vars.ContainsKey(id.Name))
                vars[id.Name]++;
            else
                vars.Add(id.Name, 0);
            base.VisitIdNode(id);
        }
    }
}
