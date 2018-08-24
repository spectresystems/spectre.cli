#!/usr/bin/env bash
script_dir=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

# Get the platform that we're running on.
uname_out="$(uname -s)"
case "${uname_out}" in
    Darwin*)    platform=osx;;
    *)          platform=linux
esac

# Create the tools directory.
tools="$script_dir/tools"
if [ ! -d "$tools" ]; then
    mkdir $tools
fi

# Make sure that cakeup exist.
cakeup="$tools/cakeup-x86_64-latest"
if [ ! -f "$cakeup" ]; then
    echo "Downloading cakeup..."
    curl -Lsfo $cakeup "https://cakeup.blob.core.windows.net/$platform/cakeup-x86_64-latest"
    if [ $? -ne 0 ]; then
        echo "An error occured while downloading cakeup."
        exit 1
    fi
    chmod +x "$cakeup"
fi

# Start Cake
exec $cakeup run --cake="0.30.0" --sdk="2.1.400" \
                 --bootstrap --execute -- $@