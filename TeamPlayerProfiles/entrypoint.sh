#!/bin/sh

# Extract arguments passed to migrations.sh
MIGRATION_NAME=${MIGRATION_NAME:-"InitialCreate"}
CONTEXT_PROJECT_NAME=${CONTEXT_PROJECT_NAME:-"DataAccess"}
STARTUP_PROJECT_NAME=${STARTUP_PROJECT_NAME:-"WebAPI"}
ADD_MIGRATION=${ADD_MIGRATION:-"false"}
APPLY_MIGRATION=${APPLY_MIGRATION:-"false"}

# Run the migrations first
echo "Running database migrations..."
./migrations.sh "$MIGRATION_NAME" "$CONTEXT_PROJECT_NAME" "$STARTUP_PROJECT_NAME" "$ADD_MIGRATION" "$APPLY_MIGRATION"

if [ $? -ne 0 ]; then
    echo "Error: Migrations failed. Exiting..."
    exit 1
fi

echo "Migrations completed successfully."

# Now start the WebAPI
echo "Starting WebAPI..."
exec dotnet WebAPI.dll  # Use exec to replace the shell with the dotnet process
