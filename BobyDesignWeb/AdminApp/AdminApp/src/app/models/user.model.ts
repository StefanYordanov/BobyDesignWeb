export interface User {
    id: string;
    userName: string;
    firstName: string;
    lastName:string;
    email: string;
    phoneNumber: string;
}

export interface RoleItem {
    role: string;
    active: boolean;
}

export enum RoleSearch {
    None = 'None',
    All = 'All',
    Admin = 'Admin',
    Seller = 'Seller'
}