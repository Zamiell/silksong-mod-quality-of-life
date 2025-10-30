#!/bin/bash

set -euo pipefail # Exit on errors and undefined variables.

SEARCH_TERM="Battle Gate"

grep "$SEARCH_TERM" "C:\Users\james\AppData\Roaming\r2modmanPlus-local\HollowKnightSilksong\profiles\Default\BepInEx\LogOutput.log" > output.txt
