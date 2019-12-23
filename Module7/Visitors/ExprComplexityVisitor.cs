using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class ExprComplexityVisitor : AutoVisitor
    {
        List<int> compList = new List<int>();

        // список должен содержать сложность каждого выражения, встреченного при обычном порядке обхода AST
        public List<int> getComplexityList()
        {
            return compList;
        }

  

        private int sum = 0;
        private int depth = 0;
        public override void VisitBinOpNode(BinOpNode binop)
        {
            depth++;
            switch (binop.Op)
            {
                case '+':
                    sum++;
                    break;
                case '-':
                    sum++;
                    break;
                case '*':
                    sum += 3;
                    break;
                default:
                    sum += 3;
                    break;
            }
            base.VisitBinOpNode(binop);
            depth--;
            if (depth == 0)
            {
                compList.Add(sum);
                sum = 0;
            }
        }

        public override void VisitIntNumNode(IntNumNode num)
        {
           if (depth == 0)
                compList.Add(sum);
        }
    }
}
