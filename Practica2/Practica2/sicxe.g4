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
    //|START EBSILON? NUMERO2 FINL
	| proposicion
	;

fin 
	:EBSILON? END entrada? FINL
	|EBSILON? END entrada?
	;
entrada 
	:ID 
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
	:CODOPF1 EBSILON?
	;
formatoDos 
	:CODOPF2 EBSILON? NUMERO2 
	|CODOPF2 EBSILON? REG 
	|CODOPF2 EBSILON? REG EBSILON? SEP REG
	|CODOPF2 EBSILON? REG EBSILON? SEP NUMERO2
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
	:CODOPF3 EBSILON? ID
	|CODOPF3 EBSILON? NUMERO2
	|CODOPF3 EBSILON? NUMERO2 EBSILON? SEP EBSILON? REG
	|CODOPF3 EBSILON? ID EBSILON? SEP EBSILON? REG
	;
indirecto3 	
	:CODOPF3 EBSILON? IND EBSILON? NUMERO2
	|CODOPF3 EBSILON? IND EBSILON? ID
	;
inmediato3 
	:CODOPF3 EBSILON? INM EBSILON? NUMERO2
	|CODOPF3 EBSILON? INM EBSILON? ID
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
	:(' '|'\r'|'\t')+
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
	: '\n'
	;




