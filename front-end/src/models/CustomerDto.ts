// export interface CustomerDto {
//   id: number;
//   name: string;
//   email: string;
// }

export interface CustomerDto {
  [key: string]: any;
  Id: number;
  Name: string;
  Address: string;
  Phone: string;
  Email: string;
}
