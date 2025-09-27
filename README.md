# RabbitMQ Showcase
## Description
This repository contains examples of using RabbitMQ in .NET 8. There are 5 examples
<ul>
    <li>
        <h3>Basic</h3>
        <p>Terms: direct exchange, default exchange, queue</p>
        <p>
            One producer sends messages to named queue (through "default exchange") and one consumer reads them from it.
        </p>
    </li>
    <li>
        <h3>Work balancing</h3>
        <p>Terms: direct exchange, default exchange, queue, work balancing</p>
        <p>
            In this one Work Queue will be used to distribute time-consuming tasks among multiple workers.
        </p>
    </li>
    <li>
        <h3>Publish/Subscribe</h3>
        <p>Terms: direct exchange, queue, "fan out" type</p>
        <p>
            Simple example of logging system. One producer pushes logs to exchange that broadcasts logs to any connected consumers (they all receive same logs).
        </p>
    </li>
    <li>
        <h3>Routing</h3>
        <p>Terms: direct exchange, routing key, queue</p>
        <p>
            Another logging system. Consumers subscribe to only a subset of the messages with help of "routing keys".c
        </p>
    </li>
    <li>
        <h3>Remote Procedure Call (RPC)</h3>
        <p>
            In this example there are two APIs: BookStore API and Inventory API. BookStore API can request collection of books from Inventory API and wait for the result.
        </p>
    </li>
</ul>

## Requirements
1. .NET 8
2. Visual Studio 2022 (v17.14.* or later)
3. Docker

## How to run
1. Open Visual Studio and use "Clone repository" option with this url:
```
https://github.com/DLiapkin/RabbitShowcase.git
```

2. You need to create running container with RabbitMQ in it
```sh
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.0-management
```

3. After that you can choose wрich example to run with drop-down menu:
<img src=".images\example_selection.png">


## Sources
<ul>
    <li>
        Official RabbitMQ <a href="https://www.rabbitmq.com/tutorials">documentation</a>
    </li>
    <li>
        Good practical youtube <a href="https://www.youtube.com/playlist?list=PLalrWAGybpB-UHbRDhFsBgXJM1g6T4IvO">playlist</a> with examples on different programming languages
    </li>
</ul>
