#!/bin/bash

# Extract arguments passed to migrations.sh with default values
MIGRATION_NAME="${USER_AUTH__MIGRATION_NAME:-"InitialCreate"}"
CONTEXT_PROJECT_NAME="${USER_AUTH__CONTEXT_PROJECT_NAME:-"EFData"}"
STARTUP_PROJECT_NAME="${USER_AUTH__STARTUP_PROJECT_NAME:-"APIAuth"}"
ADD_MIGRATION="${USER_AUTH__ADD_MIGRATION:-"false"}"
APPLY_MIGRATION="${USER_AUTH__APPLY_MIGRATION:-"false"}"

echo "addmigr: $ADD_MIGRATION"
echo "applymigr: $APPLY_MIGRATION"

# Ensure that the required environment variables are set
if [[ -z "$ADD_MIGRATION" || -z "$APPLY_MIGRATION" || -z "$MIGRATION_NAME" || -z "$CONTEXT_PROJECT_NAME" || -z "$STARTUP_PROJECT_NAME" ]]; then
    echo "Error: 'ADD_MIGRATION', 'APPLY_MIGRATION', 'MIGRATION_NAME', 'CONTEXT_PROJECT_NAME' and 'STARTUP_PROJECT_NAME' arguments must be set."
    exit 1
fi

# Install EF Core CLI tools (if either ADD_MIGRATION or APPLY_MIGRATION is set to "true")
if [[ "$ADD_MIGRATION" == "true" || "$APPLY_MIGRATION" == "true" ]]; then
    echo "Installing dotnet-ef tool..."
    dotnet tool install --global dotnet-ef
    export PATH="$PATH:/root/.dotnet/tools"
fi

# Check if the ADD_MIGRATION flag is set to "true"
if [[ "$ADD_MIGRATION" == "true" ]]; then
    # Run the migration creation command
    echo "Running migration: dotnet ef migrations add $MIGRATION_NAME -p $CONTEXT_PROJECT_NAME -s $STARTUP_PROJECT_NAME"
    dotnet ef migrations add "$MIGRATION_NAME" -p "/src/Auth/$CONTEXT_PROJECT_NAME" -s "/src/Auth/$STARTUP_PROJECT_NAME"

    # Check if the migration was successful
    if [[ $? -ne 0 ]]; then
        echo "Error: Migration failed."
        exit 1
    fi
else
    echo "Skipping migration addition."
fi

# Check if the APPLY_MIGRATION flag is set to "true"
if [[ "$APPLY_MIGRATION" == "true" ]]; then
    # Run the database update command
    echo "Updating database with migration: $MIGRATION_NAME"
    dotnet ef database update "$MIGRATION_NAME" --project "/src/Auth/$STARTUP_PROJECT_NAME"

    # Check if the database update was successful
    if [[ $? -ne 0 ]]; then
        echo "Error: Database update failed."
        exit 1
    fi
else
    echo "Skipping database update."
fi

echo "Script completed."
