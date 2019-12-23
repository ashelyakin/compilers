%{
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public BlockNode root; // �������� ���� ��������������� ������ 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union { 
			public double dVal; 
			public int iVal; 
			public string sVal; 
			public Node nVal;
			public ExprNode eVal;
			public StatementNode stVal;
			public BlockNode blVal;
			public List_identNode varVal;
       }

%using ProgramTree;

%namespace SimpleParser

%token BEGIN END CYCLE ASSIGN SEMICOLON WHILE DO REPEAT UNTIL FOR TO WRITE LEFTBRACKET RIGHTBRACKET IF THEN ELSE VAR COMMA MINUS PLUS DIVISION MULT
%token <iVal> INUM 
%token <dVal> RNUM 
%token <sVal> ID

%type <eVal> expr ident T F
%type <stVal> assign statement cycle while repeat for write if var
%type <blVal> stlist block
%type <varVal> list_ident

%%

progr   : block { root = $1; }
		;

stlist	: statement 
			{ 
				$$ = new BlockNode($1); 
			}
		| stlist SEMICOLON statement 
			{ 
				$1.Add($3); 
				$$ = $1; 
			}
		;

statement: assign { $$ = $1; }
		| block   { $$ = $1; }
		| cycle   { $$ = $1; }
		| while   { $$ = $1; }
		| repeat  { $$ = $1; }
		| for     { $$ = $1; }
		| write   { $$ = $1; }
		| if	  { $$ = $1; }
		| var	  { $$ = $1; }
	;

ident 	: ID { $$ = new IdNode($1); }	
		;
	
assign 	: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3); }
		;

/*expr	: ident  { $$ = $1 as IdNode; }
		| INUM { $$ = new IntNumNode($1); }
		;*/

block	: BEGIN stlist END { $$ = $2; }
		;

cycle	: CYCLE expr statement { $$ = new CycleNode($2, $3); }
		;

while	: WHILE expr DO statement { $$ = new WhileNode($2, $4); }
		;

repeat	: REPEAT stlist UNTIL expr { $$ = new RepeatNode($2, $4); }
		;

for		: FOR assign TO expr DO statement { $$ = new ForNode($2, $4, $6); }
		;

write	: WRITE LEFTBRACKET expr RIGHTBRACKET { $$ = new WriteNode($3); }
		;

if		: IF expr THEN statement { $$ = new IfNode($2, $4); }
		| IF expr THEN statement ELSE statement { $$ = new IfNode($2, $4, $6); }
		;

list_ident: ident { $$ = new List_identNode($1); }
		| list_ident COMMA ident
			{
				$1.Add($3);
				$$ = $1;
			}
		;

var		: VAR list_ident { $$ = new VarDefNode($2); }
		;

expr	: T { $$ = $1; }
		| expr PLUS T { $$ = new BinaryNode($1, $3, '+'); }
		| expr MINUS T { $$ = new BinaryNode($1, $3, '-'); }
		;

T		: F { $$ = $1; }
		| T MULT F { $$ = new BinaryNode($1, $3, '*'); }
		| T DIVISION F { $$ = new BinaryNode($1, $3, '/'); }
		;

F		: ident { $$ = $1 as IdNode; }
		| INUM { $$ = new IntNumNode($1); }
		| LEFTBRACKET expr RIGHTBRACKET { $$ = $2; }
		;

%%

