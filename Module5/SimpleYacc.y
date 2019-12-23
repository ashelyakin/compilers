%{
// Ёти объ€влени€ добавл€ютс€ в класс GPPGParser, представл€ющий собой парсер, генерируемый системой gppg
    public Parser(AbstractScanner<int, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%namespace SimpleParser

%token BEGIN END CYCLE INUM RNUM ID ASSIGN SEMICOLON WHILE DO REPEAT UNTIL FOR TO IF THEN ELSE WRITE LEFTBRACKET RIGHTBRACKET VAR COMMA MINUS PLUS DIVISION MULT 

%%

progr   : block
		;

stlist	: statement 
		| stlist SEMICOLON statement 
		;

statement: assign
		| block  
		| cycle 
		| while
		| repeat
		| for
		| if
		| write
		| var
		;

ident 	: ID 
		;
	
assign 	: ident ASSIGN expr 
		;

block	: BEGIN stlist END 
		;

cycle	: CYCLE expr statement 
		;

while	: WHILE expr DO statement
		;

repeat	: REPEAT stlist UNTIL expr
		;

for		: FOR assign TO expr DO statement
		;

if		: IF expr THEN statement
		| IF expr THEN statement ELSE statement
		;

write	: WRITE LEFTBRACKET expr RIGHTBRACKET	
		;

list_ident: ident COMMA list_ident
		| ident
		;

var		: VAR list_ident 
		;

expr	: T
		| expr PLUS T
		| expr MINUS T
		;

T		: F
		| T MULT F
		| T DIVISION F
		;

F		: ident
		| INUM 
		| LEFTBRACKET expr RIGHTBRACKET
		;

%%
