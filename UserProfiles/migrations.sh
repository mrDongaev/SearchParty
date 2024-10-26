#!/bin/sh

# Extract arguments passed to migrations.sh
MIGRATION_NAME=${USER_PROFILES__MIGRATION_NAME:-"InitialCreate"}
CONTEXT_PROJECT_NAME=${USER_PROFILES__CONTEXT_PROJECT_NAME:-"DataAccess"}
STARTUP_PROJECT_NAME=${USER_PROFILES__STARTUP_PROJECT_NAME:-"WebAPI"}
ADD_MIGRATION=${USER_PROFILES__ADD_MIGRATION:-"false"}
APPLY_MIGRATION=${USER_PROFILES__APPLY_MIGRATION:-"false"}

echo "addmigr: $ADD_MIGRATION"
echo "applymigr: $APPLY_MIGRATION"

# Ensure that the required environment variables are set
if [ -z "$ADD_MIGRATION" ] || [ -z "$APPLY_MIGRATION" ] ||[ -z "$MIGRATION_NAME" ] || [ -z "$CONTEXT_PROJECT_NAME" ] || [ -z "$STARTUP_PROJECT_NAME" ]; then
    echo "Error: 'ADD_MIGRATION', 'APPLY_MIGRATION', 'MIGRATION_NAME', 'CONTEXT_PROJECT_NAME' and 'STARTUP_PROJECT_NAME' arguments must be set."
    exit 1
fi

# Install EF Core CLI tools (if either ADD_MIGRATION or APPLY_MIGRATION is set to "true")
if [ "$ADD_MIGRATION" = "true" ] || [ "$APPLY_MIGRATION" = "true" ]; then
    echo "Installing dotnet-ef tool..."
    dotnet tool install --global dotnet-ef
    export PATH="$PATH:/root/.dotnet/tools"
    export APPLY_MIGRATION=$APPLY_MIGRATION
fi

# Check if the ADD_MIGRATION flag is set to "true"
if [ "$ADD_MIGRATION" = "true" ]; then
    # Run the migration creation command
    echo "Running migration: dotnet ef migrations add $MIGRATION_NAME -p $CONTEXT_PROJECT_NAME -s $STARTUP_PROJECT_NAME"
    dotnet ef migrations add "$MIGRATION_NAME" -p "/src/$CONTEXT_PROJECT_NAME" -s "/src/$STARTUP_PROJECT_NAME"

    # Check if the migration was successful
    if [ $? -ne 0 ]; then
        echo "Error: Migration failed."
        exit 1
    fi
else
    echo "Skipping migration addition."
fi

# Check if the APPLY_MIGRATION flag is set to "true"
if [ "$APPLY_MIGRATION" = "true" ]; then
    # Run the database update command
    echo "Updating database with migration: $MIGRATION_NAME"
    dotnet ef database update "$MIGRATION_NAME" --project "/src/$STARTUP_PROJECT_NAME"

    # Check if the database update was successful
    if [ $? -ne 0 ]; then
        echo "Error: Database update failed."
        exit 1
    fi
else
    echo "Skipping database update."
fi

echo "Script completed."