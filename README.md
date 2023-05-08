# RabbitMQ Story

## Сеть для сервисов
1. Создаем `docker network create test-network`.

## RabbitMQ
1. Запуск `docker run -d -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password --name some-rabbit --network=test-network rabbitmq:3-management`
2. Проверяем запуск, прыгаем в http://localhost:15672/.

## Producer API

1. Сборка образа `docker build . -t producer-api:<tag>`
2. Запуск `docker run -d -p 90:80 --name producer --network=test-network producer-api:<tag>`, для взаимодействия с Swagger'ом можем указать среду разработки `docker run -d -p 90:80 -e ASPNETCORE_ENVIRONMENT=Development --name producer --network=test-network producer-api:<tag>`
3. Проверяем, через Swagger: http://localhost:90/swagger/index.html, либо сторонним клиентом Post: http://localhost:90/api/rabbitmq/send.

## Consumer API

1. Сборка образа `docker build . -t consumer-api:<tag>`
2. Запуск `docker run -d -p 91:80 --name consumer --network=test-network -e ASPNETCORE_ENVIRONMENT=Development consumer-api:<tag>`.
3. Отправляем сообщения через producer, проверяем логи: `docker logs consumer`.

## Run with docker compose 

`docker compose up -d`

при этом следим за состоянием подписчика, на начале запуска, он пару раз упадет, потом запустится нормально, когда rabbitMq будет готов.
Отправить сообщение в очередь: http://localhost:90/swagger/index.html, если `Development`, иначе через Post: http://localhost:90/api/rabbitmq/send.
UI rabbit: http://localhost:15672/.
Проверить получение сообщения подписчиком: `docker logs consumer`.