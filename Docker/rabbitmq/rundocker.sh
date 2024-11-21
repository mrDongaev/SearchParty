#!/bin/bash

# Define the image and container name
IMAGE_NAME="search-party-rabbitmq"
CONTAINER_NAME="search-party-message-bus"

# Build the Docker image from the Dockerfile
echo "Building the Docker image..."
docker build -t $IMAGE_NAME .

# Check if a container with the same name is already running
if [ "$(docker ps -q -f name=$CONTAINER_NAME)" ]; then
    echo "Stopping and removing existing container..."
    docker stop $CONTAINER_NAME
    docker rm $CONTAINER_NAME
fi

# Run the container from the built image
echo "Running the container from the image $IMAGE_NAME..."
docker run --name $CONTAINER_NAME -p 5672:5672 -p 15672:15672 $IMAGE_NAME
