# Changelog

All notable changes to this project are documented in this file.

## [3.0.0-alpha.1] - to be released

### Breaking Changes

- `IErrorOr.Errors`, `ErrorOr.Errors` and `ErrorOr.ErrorsOrEmptyList` changes type from `List<Error>` to `ReadOnlyCollection<Error>`

Consequently all `Match`, `Switch` and `Else` methods and extension methods that accepted delegates with `List<Error>` as first argument now accepts delegates with `ReadOnlyCollection<Error>` as first argument.

`Match`, `MatchAsync` methods and extension methods:

```diff
     public TNextValue Match<TNextValue>(
         Func<TValue, TNextValue> onValue,
-        Func<List<Error>, TNextValue> onError)
+        Func<ReadOnlyCollection<Error>, TNextValue> onError)
```

```diff
     public async Task<TNextValue> MatchAsync<TNextValue>(
         Func<TValue, Task<TNextValue>> onValue,
-        Func<List<Error>, Task<TNextValue>> onError)
+        Func<ReadOnlyCollection<Error>, Task<TNextValue>> onError)
```

```diff
     public static async Task<TNextValue> Match<TValue, TNextValue>(
         this Task<ErrorOr<TValue>> errorOr,
         Func<TValue, TNextValue> onValue,
-        Func<List<Error>, TNextValue> onError)
+        Func<ReadOnlyCollection<Error>, TNextValue> onError)
```

```diff
     public static async Task<TNextValue> MatchAsync<TValue, TNextValue>(
         this Task<ErrorOr<TValue>> errorOr,
         Func<TValue, Task<TNextValue>> onValue,
-        Func<List<Error>, Task<TNextValue>> onError)
+        Func<ReadOnlyCollection<Error>, Task<TNextValue>> onError)
```

`Switch`, `SwitchAsync` methods and extension methods:

```diff
     public void Switch(
         Action<TValue> onValue,
-        Action<List<Error>> onError)
+        Action<ReadOnlyCollection<Error>> onError)
```

```diff
     public async Task SwitchAsync(
         Func<TValue, Task> onValue,
-        Func<List<Error>, Task> onError)
+        Func<ReadOnlyCollection<Error>, Task> onError)
```

```diff
     public static async Task Switch<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
         Action<TValue> onValue,
-        Action<List<Error>> onError)
+        Action<ReadOnlyCollection<Error>> onError)
```

```diff
     public static async Task SwitchAsync<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
         Func<TValue, Task> onValue,
-        Func<List<Error>, Task> onError)
+        Func<ReadOnlyCollection<Error>, Task> onError)
```

`Else`, `ElseAsync` methods and extension methods:

```diff
-    public ErrorOr<TValue> Else(Func<List<Error>, Error> onError)
+    public ErrorOr<TValue> Else(Func<ReadOnlyCollection<Error>, Error> onError)
```

```diff
-    public ErrorOr<TValue> Else(Func<List<Error>, List<Error>> onError)
+    public ErrorOr<TValue> Else(Func<ReadOnlyCollection<Error>, List<Error>> onError)
```

```diff
-    public ErrorOr<TValue> Else(Func<List<Error>, TValue> onError)
+    public ErrorOr<TValue> Else(Func<ReadOnlyCollection<Error>, TValue> onError)
```

```diff
-    public async Task<ErrorOr<TValue>> ElseAsync(Func<List<Error>, Task<TValue>> onError)
+    public async Task<ErrorOr<TValue>> ElseAsync(Func<ReadOnlyCollection<Error>, Task<TValue>> onError)
```

```diff
-    public async Task<ErrorOr<TValue>> ElseAsync(Func<List<Error>, Task<Error>> onError)
+    public async Task<ErrorOr<TValue>> ElseAsync(Func<ReadOnlyCollection<Error>, Task<Error>> onError)
```

```diff
-    public async Task<ErrorOr<TValue>> ElseAsync(Func<List<Error>, Task<List<Error>>> onError)
+    public async Task<ErrorOr<TValue>> ElseAsync(Func<ReadOnlyCollection<Error>, Task<List<Error>>> onError)
```

```diff
     public static async Task<ErrorOr<TValue>> Else<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
-        Func<List<Error>, TValue> onError)
+        Func<ReadOnlyCollection<Error>, TValue> onError)
```

```diff
     public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
-        Func<List<Error>, Task<TValue>> onError)
+        Func<ReadOnlyCollection<Error>, Task<TValue>> onError)
```

```diff
     public static async Task<ErrorOr<TValue>> Else<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
-        Func<List<Error>, Error> onError)
+        Func<ReadOnlyCollection<Error>, Error> onError)
```

```diff
     public static async Task<ErrorOr<TValue>> Else<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
-        Func<List<Error>, List<Error>> onError)
+        Func<ReadOnlyCollection<Error>, List<Error>> onError)
```

```diff
     public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
-        Func<List<Error>, Task<Error>> onError)
+        Func<ReadOnlyCollection<Error>, Task<Error>> onError)
```

```diff
     public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(
         this Task<ErrorOr<TValue>> errorOr,
-        Func<List<Error>, Task<List<Error>>> onError)
+        Func<ReadOnlyCollection<Error>, Task<List<Error>>> onError)
```

- `ErrorOr<TValue>.From` method that accepted `List<Error>` now accepts `IEnumerable<Error>`

```diff
-    public static ErrorOr<TValue> From(List<Error> errors)
+    public static ErrorOr<TValue> From(IEnumerable<Error> errors)
```

- `Error.Metadata` property type chages from `Dictionary<string, object>` to `ReadOnlyDictionary<string,object>`

- Static factory methods of `Error` accepts `IDictionary<string,object>` instead of `Dictionary<string,object>`

```diff
     public static Error Failure(
         string code = "General.Failure",
         string description = "A failure has occurred.",
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

```diff
     public static Error Unexpected(
         string code = "General.Unexpected",
         string description = "An unexpected error has occurred.",
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

```diff
     public static Error Validation(
         string code = "General.Validation",
         string description = "A validation error has occurred.",
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

```diff
     public static Error Conflict(
         string code = "General.Conflict",
         string description = "A conflict error has occurred.",
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

```diff
     public static Error NotFound(
         string code = "General.NotFound",
         string description = "A 'Not Found' error has occurred.",
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

```diff
     public static Error Unauthorized(
         string code = "General.Unauthorized",
         string description = "An 'Unauthorized' error has occurred.",
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

```diff
     public static Error Forbidden(
         string code = "General.Forbidden",
         string description = "A 'Forbidden' error has occurred.",
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

```diff
    public static Error Custom(
         int type,
         string code,
         string description,
-        Dictionary<string, object>? metadata = null)
+        IDictionary<string, object>? metadata = null)
```

### Added

- New `ToErrorOr` extension method was added for `ReadOnlyCollection<Error>`

```csharp
ReadOnlyCollection<Error> errors = new([Error.Unauthorized(), Error.Validation()]);
ErrorOr<int> result = errors.ToErrorOr<int>();
```

- New `ErrorOr` implicit converter was added for `ReadOnlyCollection<Error>`

```csharp
ReadOnlyCollection<Error> errors = new([Error.Unauthorized(), Error.Validation()]);
ErrorOr<int> result = errors;
```

## [2.0.0] - 2024-03-26

### Added

- `FailIf`

```csharp
public ErrorOr<TValue> FailIf(Func<TValue, bool> onValue, Error error)
```

```csharp
ErrorOr<int> errorOr = 1;
errorOr.FailIf(x => x > 0, Error.Failure());
```

### Breaking Changes

- `Then` that receives an action is now called `ThenDo`

```diff
-public ErrorOr<TValue> Then(Action<TValue> action)
+public ErrorOr<TValue> ThenDo(Action<TValue> action)
```

```diff
-public static async Task<ErrorOr<TValue>> Then<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<TValue> action)
+public static async Task<ErrorOr<TValue>> ThenDo<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<TValue> action)
```

- `ThenAsync` that receives an action is now called `ThenDoAsync`

```diff
-public async Task<ErrorOr<TValue>> ThenAsync(Func<TValue, Task> action)
+public async Task<ErrorOr<TValue>> ThenDoAsync(Func<TValue, Task> action)
```

```diff
-public static async Task<ErrorOr<TValue>> ThenAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task> action)
+public static async Task<ErrorOr<TValue>> ThenDoAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task> action)
```

## [1.10.0] - 2024-02-14

### Added

- `ErrorType.Forbidden`
- README to NuGet package

## [1.9.0] - 2024-01-06

### Added

- `ToErrorOr`
