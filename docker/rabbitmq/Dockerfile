# Use the official RabbitMQ image from Docker Hub
FROM rabbitmq:3-management

# Expose the ports needed for RabbitMQ and the management client
EXPOSE 5672 15672

# Set default environment variables (optional)
ENV RABBITMQ_DEFAULT_USER=guest
ENV RABBITMQ_DEFAULT_PASS=guest

# Start RabbitMQ server when the container starts
CMD ["rabbitmq-server"]