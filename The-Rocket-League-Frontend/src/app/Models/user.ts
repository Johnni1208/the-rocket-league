import { Rocket } from './rocket';

export interface User {
  id: number;
  username: string;
  password: string;
  rockets: Rocket[];
}
