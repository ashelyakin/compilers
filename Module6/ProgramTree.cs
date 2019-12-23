using System.Collections.Generic;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public class Node // базовый класс для всех узлов    
    {
    }

    public class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
    }

    public class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
    }

    public class WhileNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public WhileNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
    }

    public class RepeatNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public List<StatementNode> StList = new List<StatementNode>();
        public RepeatNode(StatementNode stlist, ExprNode expr)
        {
            Expr = expr;
            StList.Add(stlist);
        }
    }

    public class ForNode : StatementNode
    {
        public StatementNode Assign { get; set; }
        public ExprNode Expr { get; set; }
        public StatementNode Stlist { get; set; }
        public ForNode(StatementNode a, ExprNode expr, StatementNode stat)
        {
            Assign = a;
            Expr = expr;
            Stlist = stat;
        }
    }

    public class WriteNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public WriteNode(ExprNode expr)
        {
            Expr = expr;
        }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat_first { get; set; }
        public StatementNode Stat_second { get; set; }
        public IfNode(ExprNode expr, StatementNode stat_first, StatementNode stat_second)
        {
            Expr = expr;
            Stat_first = stat_first;
            Stat_second = stat_second;
        }
        public IfNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat_first = stat;
        }
    }

    public class List_identNode : StatementNode
    {
        public LinkedList<ExprNode> List_ident = new LinkedList<ExprNode>();
        public List_identNode(ExprNode expr)
        {
            Add(expr);
        }
        public void Add(ExprNode expr)
        {
            List_ident.AddLast(expr);
        }
    }
    public class VarDefNode : StatementNode
    {
        public List_identNode List_ident { get; set; }
        public VarDefNode(List_identNode list_ident)
        {
            List_ident = list_ident;
        }
    }

    public class BinaryNode : ExprNode
    {
        public ExprNode Expr1 { get; set; }
        public ExprNode Expr2 { get; set; }
        public char Oper { get; set; }
        public BinaryNode(ExprNode expr1, ExprNode expr2, char oper)
        {
            Expr1 = expr1;
            Expr2 = expr2;
            Oper = oper;
        }
    }
}