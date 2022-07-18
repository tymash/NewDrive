export interface UserRegisterModel {
  name: string,
  surname: string,
  email: string,
  password: string
}

export interface UserLoginModel {
  email: string,
  password: string
}

export interface UserEditModel {
  id?: string,
  name?: string,
  surname?: string,
  email?: string
}

export interface UserChangePasswordModel {
  id: string,
  password: string
}

export interface UserViewModel {
  id: number,
  email: string,
  name: string,
  surname: string,
  filesIds: number[],
  token: string
}