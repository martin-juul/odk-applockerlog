export class User {
    constructor(
        public userName: string,
        public incidentReaderGroup: string,
        public incidentResolverGroup: string,
        public userReaderGroup: string
    ) { }
}
