// import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Document } from 'mongoose';
import * as mongoose from 'mongoose';

export class CreatePersonDTO {
  name: string;
  dodid: number;
  afscid: number;
  workCenter: string;
  timeOnStation: Date
  status: string;
  dueDate: Date;
  comments: string;
}

export const PersonSchema = new mongoose.Schema({
  name: String,
  dodid: Number,
  afscid: Number,
  workCenter: String,
  timeOnStation: Date,
  status: String,
  dueDate: Date,
  comments: String
});

export interface Person extends Document, CreatePersonDTO  {}

// // Need to declare this before it's used
// export enum PersonStatus {
//   CURRENT = 'Current',
//   UPCOMING = 'Upcoming',
//   OVERDUE = 'Overdue',
// }
