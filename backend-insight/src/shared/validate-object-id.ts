import { PipeTransform, Injectable, ArgumentMetadata, BadRequestException } from '@nestjs/common';
import * as mongoose from 'mongoose';

@Injectable()
export class ValidateObjectID implements PipeTransform<string> {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  async transform(value: string, metadata: ArgumentMetadata) {
    const isValid = mongoose.Types.ObjectId.isValid(value);
    if (!isValid) {
        throw new BadRequestException('Invalid ID!');
    }
    return value;
  }
}

// @Injectable()
// export class ValidateDoDID implements PipeTransform<string> {
//   // eslint-disable-next-line @typescript-eslint/no-unused-vars
//   async transform(value: string, metadata: ArgumentMetadata) {

//     const isValid = mongoose.Types.
//     if (!isValid) {
//         throw new BadRequestException('Invalid ID!');
//     }
//     return value;
//   }
// }
@Injectable()
export class ValidateSearch implements PipeTransform<string> {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  async transform(value: string, metadata: ArgumentMetadata) {
    const isValid = mongoose.Types.ObjectId.isValid(value);
    if (!isValid) {
        throw new BadRequestException('No Results Found');
    }
    return value;
  }
}