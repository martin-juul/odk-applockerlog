export class Software {
    constructor(
       public id: number,
       public name: string,
       public vendor: string,
       public reasoning: string,
       public timeStamp: string,
       public createdBy: string,
       public state: string
    ) { }
}
