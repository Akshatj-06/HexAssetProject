export class UserModel {
    userId: number;
    role: string;
    name: string;
    email: string;
    password: string;
    contactNumber: number;
    address: string;

    constructor(){
        this.userId=0;
        this.role= "";
        this.name="";
        this.email="";
        this.password="";
        this.contactNumber=0;
        this.address="";
    }
}