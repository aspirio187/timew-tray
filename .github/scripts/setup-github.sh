#!/usr/bin/env bash
# Configure GitHub labels for Timew Tray.
# Prerequisites: gh CLI installed and authenticated.
#
# Usage:
#   GH_REPO=owner/timew-tray bash .github/scripts/setup-github.sh

set -euo pipefail

REPO="${GH_REPO:-aspirio187/timew-tray}"

echo "Setting up GitHub labels for: $REPO"

create_label() {
  local name="$1"
  local color="$2"
  local description="$3"

  gh label create "$name" \
    --repo "$REPO" \
    --color "$color" \
    --description "$description" \
    --force
}

create_label "type:feature" "a2eeef" "New functionality"
create_label "type:fix" "d73a4a" "Bug fix"
create_label "type:docs" "0075ca" "Documentation only"
create_label "type:ci" "bfd4f2" "CI or repository automation"
create_label "type:packaging" "c5def5" "Installers, publishing, or platform packaging"

create_label "area:avalonia" "5319e7" "Avalonia UI, tray, views, or view models"
create_label "area:timewarrior" "0e8a16" "Timewarrior command integration or parsing"
create_label "area:platform-linux" "f9d0c4" "Linux-specific behavior"
create_label "area:platform-macos" "fbca04" "macOS-specific behavior"
create_label "area:platform-windows" "c2e0c6" "Windows-specific behavior"
create_label "area:tests" "d4c5f9" "Automated or manual tests"

create_label "priority:high" "e11d48" "Important or blocking"
create_label "priority:medium" "f97316" "Normal priority"
create_label "priority:low" "84cc16" "Polish or future improvement"

echo "Labels created or updated."

