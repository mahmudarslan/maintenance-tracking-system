export class DocNumberDto {
   id?: string;
   docNoType: number;
   nextNumber: number;
   minDigits: number;
   prefix: string;
   suffix: string;
}

export class DocTypeDto {
   id?: number;
   name: string;
}