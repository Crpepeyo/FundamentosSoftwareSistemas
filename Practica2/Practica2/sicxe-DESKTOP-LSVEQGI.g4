grammar sicxe;


options {							
    language=CSharp2;								//lenguaje objetivo de la gramatica
}

/*
*	Reglas del Parser
*/
programa 					
	: inicio proposiciones fin
	;
	
inicio 	
	: (etiqueta? EBSILON? START EBSILON? NUMERO2 FINL) 
	| proposicion
	;

fin 
	:END etiqueta? FINL
	|END etiqueta?
	;
	
proposiciones 
	: (proposicion)( (proposicion)*)
	;

proposicion 
	:instruccion
	|directiva
	;

instruccion	
	:etiqueta? EBSILON? opinstruccion FINL 
	;
	
directiva 
	:etiqueta? EBSILON? tipoDirectiva EBSILON? opDirectiva FINL
	;
tipoDirectiva 	
	:BYTE | WORD | RESB | RESW | BASE
	;

etiqueta 
	:ID 
	;
opinstruccion 
	:formato
	;
formato
	:formatoUno
	|formatoDos
	|formatoTres
	|formatoCuatro
	;
formatoUno 	
	:CODOPF1
	;
formatoDos 
	:CODOPF2 NUMERO2 
	|CODOPF2 REG 
	|CODOPF2 REG SEP REG
	|CODOPF2 REG SEP NUMERO2
	;		
formatoTres 
	:simple3
	|indirecto3
	|inmediato3
	;
formatoCuatro 
	:F4 (simple3 
	|indirecto3
	|inmediato3)
	;

simple3 
	:CODOPF3 ID
	|CODOPF3 NUMERO2
	|CODOPF3 NUMERO2 SEP REG
	|CODOPF3 ID SEP REG
	;
indirecto3 	
	:CODOPF3 IND NUMERO2
	|CODOPF3 IND ID
	;
inmediato3 
	:CODOPF3 INM NUMERO2
	|CODOPF3 INM ID
	;
opDirectiva 
	:NUMERO2 
	| CONSTHEX 
	| CONSTCAD
	| ID
	;

registro 	
	:REG
	;


CODOPF1
	:'FIX'|'RSUB'
	|'FLOAT'|'HIO'|'NORM'|'SIO'|'TIO'
	;
CODOPF2
	:'ADDR'|'CLEAR'|'COMPR'|'DIVR'|'RMO'
	|'SHIFTL'|'SHIFTR'|'SUBR'
	|'SVC'|'TIXR'
	;
CODOPF3
	:'ADD'|'ADDF'|'AND'|'COMP'
	|'COMPF'|'DIV'|'DIVF'|'J'|'JEQ'|'JGT'|'JLT'|'JSUB'  
	|'LDA'|'LDB'|'LDCH'|'LDF'|'LDL'|'LDS'|'LDT'
	|'LDX'|'LPS'|'MUL'|'MULF'|'MULR'|'OR'
	|'RD'
	|'SSK'|'STA'|'STB'|'STCH'|'STF'|'STI'|'STL'
	|'STS'|'STSW'|'STT'|'STX'|'SUB'|'SUBF'|'TD'|'TIX'|'WD'
	;

REG
	:'A'|'X'|'L'|'B'|'S'|'T'
	|'F'|'CP'|'SW'
	;


START 
	:'START'
	;

BYTE
	:'BYTE' 
	;
WORD 
	:'WORD'
	;
RESB
	:'RESB'
	;
RESW
	:'RESW'
	;
BASE
	:'BASE'
	;
END
	: 'END'
	; 

SEP
	:', '
	;
F4
	:'+'
	;
EBSILON
	:[ \r\t]+->skip
	;

IND
	:'@'
	;

INM
	:'#'
	;

CONSTHEX
	:('X')'\''('0'..'9'|'A'..'Z')+'\''
	;

CONSTCAD
	:('C')'\''('a'..'z'|'A'..'Z')+'\''
	;

NUMERO2
    :('0'..'9')+'H'|('0'..'9'|'A'..'F')+'H'|('0'..'9')+
	;	
ID
	:('A'..'Z'|'0'..'9')+' ' 
	|('A'..'Z'|'0'..'9')+
	;

/*CONSTCAD
	:('A'..'Z')+('0'..'9')+
	;*/



FINL
	: [\n]+
	;




