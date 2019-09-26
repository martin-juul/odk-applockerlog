/*
    Usage:
    let approval = new Approval(
        data[i].userName,
        data[i].reasoning,
        data[i].timeStamp,
        data[i].approver,
        new AssignedUserGroup {
            data[i].groupName
        }
    );
*/

export class Approval {
    constructor(
        public id: number,
        public userName: string,
        public compterName: string,
        public reasoning: string,
        public timeStamp: string,
        public approver: string,
        public assignedUserGroups: AssignedUserGroup[]
    ) { }
}

export class AssignedUserGroup {
    constructor(
        public id: number,
        public approvalId: number,
        public group: string
    ) { }
}
