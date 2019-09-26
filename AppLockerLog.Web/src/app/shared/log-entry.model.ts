export class LogEntry {
    constructor(
        public id: number,
        public userName: string,
        public computerName: string,
        public ip: string,
        public softwareName: string,
        public programDescription: string,
        public rapportDescription: string,
        public timeStamp: string,
        public note: string,
        public editedBy: string,
        public deniedBy: string) { }
}
