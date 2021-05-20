import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);


  app.enableCors();

  // Change port so frontend is 3000
  await app.listen(5000);
}

bootstrap();
