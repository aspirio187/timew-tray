---
applyTo: '**/*.axaml,**/*.axaml.cs,**/Views/**,**/ViewModels/**'
---

# Avalonia Instructions

Use these rules for Avalonia UI, views, and view models.

## UI Architecture

- Follow MVVM.
- Keep view code-behind minimal and limited to Avalonia wiring that cannot be expressed through bindings.
- Put application state and commands in view models.
- Keep Timewarrior command execution in services, never directly in views.
- Do not block the UI thread.

## Tray and Window Behavior

- Keep the tray UI simple: current status, active tags, elapsed duration, refresh/error state, and explicit actions only when requested.
- Make platform-specific tray, notification, and startup behavior replaceable behind interfaces.
- Handle platforms that do not support a requested tray feature gracefully.

## Bindings and State

- Prefer clear view-model properties over converter-heavy XAML.
- Represent loading, unavailable, active, inactive, and error states explicitly.
- Make error text actionable without dumping raw stack traces into the UI.

## Accessibility

- Use readable labels and tooltips for tray/menu actions.
- Do not rely on color alone to communicate active/inactive/error state.
