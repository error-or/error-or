<div align="center">

<img src="assets/icon.png" alt="drawing" width="700px"/></br>

[![NuGet](https://img.shields.io/nuget/v/erroror.svg)](https://www.nuget.org/packages/erroror)

[![Build](https://github.com/amantinband/error-or/actions/workflows/build.yml/badge.svg)](https://github.com/amantinband/error-or/actions/workflows/build.yml) [![publish ErrorOr to nuget](https://github.com/amantinband/error-or/actions/workflows/publish.yml/badge.svg)](https://github.com/amantinband/error-or/actions/workflows/publish.yml)

[![GitHub contributors](https://img.shields.io/github/contributors/amantinband/error-or)](https://GitHub.com/amantinband/error-or/graphs/contributors/) [![GitHub Stars](https://img.shields.io/github/stars/amantinband/error-or.svg)](https://github.com/amantinband/error-or/stargazers) [![GitHub license](https://img.shields.io/github/license/amantinband/error-or)](https://github.com/amantinband/error-or/blob/main/LICENSE)
[![codecov](https://codecov.io/gh/amantinband/error-or/branch/main/graph/badge.svg?token=DR2EBIWK7B)](https://codecov.io/gh/amantinband/error-or)
---

### A simple, fluent discriminated union of an error or a result

`dotnet add package ErrorOr`

</div>

- [Give it a star ⭐!](#give-it-a-star-)
- [Getting Started 🏃](#getting-started-)
  - [Replace throwing exceptions with `ErrorOr<T>`](#replace-throwing-exceptions-with-errorort)
  - [Support For Multiple Errors](#support-for-multiple-errors)
  - [Various Functional Methods and Extension Methods](#various-functional-methods-and-extension-methods)
    - [Real world example](#real-world-example)
    - [Simple Example with intermediate steps](#simple-example-with-intermediate-steps)
      - [No Failure](#no-failure)
      - [Failure](#failure)
- [Creating an `ErrorOr` instance](#creating-an-erroror-instance)
  - [Using implicit conversion](#using-implicit-conversion)
  - [Using The `ErrorOrFactory`](#using-the-errororfactory)
  - [Using The `ToErrorOr` Extension Method](#using-the-toerroror-extension-method)
- [Properties](#properties)
  - [`IsError`](#iserror)
  - [`Value`](#value)
  - [`Errors`](#errors)
  - [`FirstError`](#firsterror)
  - [`ErrorsOrEmptyList`](#errorsoremptylist)
- [Methods](#methods)
  - [`Match`](#match)
    - [`Match`](#match-1)
    - [`MatchAsync`](#matchasync)
    - [`MatchFirst`](#matchfirst)
    - [`MatchFirstAsync`](#matchfirstasync)
  - [`Switch`](#switch)
    - [`Switch`](#switch-1)
    - [`SwitchAsync`](#switchasync)
    - [`SwitchFirst`](#switchfirst)
    - [`SwitchFirstAsync`](#switchfirstasync)
  - [`Then`](#then)
    - [`Then`](#then-1)
    - [`ThenAsync`](#thenasync)
    - [`ThenDo` and `ThenDoAsync`](#thendo-and-thendoasync)
    - [Mixing `Then`, `ThenDo`, `ThenAsync`, `ThenDoAsync`](#mixing-then-thendo-thenasync-thendoasync)
  - [`FailIf`](#failif)
  - [`Else`](#else)
    - [`Else`](#else-1)
    - [`ElseAsync`](#elseasync)
    - [`ElseDo` and `ElseDoAsync`](#elsedo-and-elsedoasync)
- [Mixing Features (`Then`, `FailIf`, `Else`, `Switch`, `Match`)](#mixing-features-then-failif-else-switch-match)
- [Error Types](#error-types)
  - [Built in error types](#built-in-error-types)
  - [Custom error types](#custom-error-types)
- [Built in result types (`Result.Success`, ..)](#built-in-result-types-resultsuccess-)
- [Organizing Errors](#organizing-errors)
- [Mediator + FluentValidation + `ErrorOr` 🤝](#mediator--fluentvalidation--erroror-)
- [Contribution 🤲](#contribution-)
- [Credits 🙏](#credits-)
- [License 🪪](#license-)

# Give it a star ⭐

Loving it? Show your support by giving this project a star!

# Getting Started 🏃

## Replace throwing exceptions with `ErrorOr<T>`

This 👇

```cs
public float Divide(int a, int b)
{
    if (b == 0)
    {
        throw new Exception("Cannot divide by zero");
    }

    return a / b;
}

try
{
    var result = Divide(4, 2);
    Console.WriteLine(result * 2); // 4
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    return;
}
```

Turns into this 👇

```cs
public ErrorOr<float> Divide(int a, int b)
{
    if (b == 0)
    {
        return Error.Unexpected(description: "Cannot divide by zero");
    }

    return a / b;
}

var result = Divide(4, 2);

if (result.IsError)
{
    Console.WriteLine(result.FirstError.Description);
    return;
}

Console.WriteLine(result.Value * 2); // 4
```

Or, using [Then](#then--thenasync)/[Else](#else--elseasync) and [Switch](#switch--switchasync)/[Match](#match--matchasync), you can do this 👇

```cs

Divide(4, 2)
    .Then(val => val * 2)
    .SwitchFirst(
        onValue: Console.WriteLine, // 4
        onFirstError: error => Console.WriteLine(error.Description));
```

## Support For Multiple Errors

Internally, the `ErrorOr` object has a list of `Error`s, so if you have multiple errors, you don't need to compromise and have only the first one.

```cs
public class User(string _name)
{
    public static ErrorOr<User> Create(string name)
    {
        List<Error> errors = [];

        if (name.Length < 2)
        {
            errors.Add(Error.Validation(description: "Name is too short"));
        }

        if (name.Length > 100)
        {
            errors.Add(Error.Validation(description: "Name is too long"));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(Error.Validation(description: "Name cannot be empty or whitespace only"));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new User(name);
    }
}
```

## Various Functional Methods and Extension Methods

The `ErrorOr` object has a variety of methods that allow you to work with it in a functional way.

This allows you to chain methods together, and handle the result in a clean and concise way.

### Real world example

```cs
return await _userRepository.GetByIdAsync(id)
    .Then(user => user.IncrementAge()
        .Then(success => user)
        .Else(errors => Error.Unexpected("Not expected to fail")))
    .FailIf(user => !user.IsOverAge(18), UserErrors.UnderAge)
    .ThenDo(user => _logger.LogInformation($"User {user.Id} incremented age to {user.Age}"))
    .ThenAsync(user => _userRepository.UpdateAsync(user))
    .Match(
        _ => NoContent(),
        errors => errors.ToActionResult());
```

### Simple Example with intermediate steps

#### No Failure

```cs
ErrorOr<string> foo = await "2".ToErrorOr()
    .Then(int.Parse) // 2
    .FailIf(val => val > 2, Error.Validation(description: $"{val} is too big") // 2
    .ThenDoAsync(Task.Delay) // Sleep for 2 milliseconds
    .ThenDo(val => Console.WriteLine($"Finished waiting {val} milliseconds.")) // Finished waiting 2 milliseconds.
    .ThenAsync(val => Task.FromResult(val * 2)) // 4
    .Then(val => $"The result is {val}") // "The result is 4"
    .Else(errors => Error.Unexpected(description: "Yikes")) // "The result is 4"
    .MatchFirst(
        value => value, // "The result is 4"
        firstError => $"An error occurred: {firstError.Description}");
```

#### Failure

```cs
ErrorOr<string> foo = await "5".ToErrorOr()
    .Then(int.Parse) // 5
    .FailIf(val => val > 2, Error.Validation(description: $"{val} is too big") // Error.Validation()
    .ThenDoAsync(Task.Delay) // Error.Validation()
    .ThenDo(val => Console.WriteLine($"Finished waiting {val} milliseconds.")) // Error.Validation()
    .ThenAsync(val => Task.FromResult(val * 2)) // Error.Validation()
    .Then(val => $"The result is {val}") // Error.Validation()
    .Else(errors => Error.Unexpected(description: "Yikes")) // Error.Unexpected()
    .MatchFirst(
        value => value,
        firstError => $"An error occurred: {firstError.Description}"); // An error occurred: Yikes
```

# Creating an `ErrorOr` instance

## Using implicit conversion

There are implicit converters from `TResult`, `Error`, `List<Error>` to `ErrorOr<TResult>`

```cs
ErrorOr<int> result = 5;
ErrorOr<int> result = Error.Unexpected();
ErrorOr<int> result = [Error.Validation(), Error.Validation()];
```

```cs
public ErrorOr<int> IntToErrorOr()
{
    return 5;
}
```

```cs
public ErrorOr<int> SingleErrorToErrorOr()
{
    return Error.Unexpected();
}
```

```cs
public ErrorOr<int> MultipleErrorsToErrorOr()
{
    return [
        Error.Validation(description: "Invalid Name"),
        Error.Validation(description: "Invalid Last Name")
    ];
}
```

## Using The `ErrorOrFactory`

```cs
ErrorOr<int> result = ErrorOrFactory.From(5);
ErrorOr<int> result = ErrorOrFactory.From<int>(Error.Unexpected());
ErrorOr<int> result = ErrorOrFactory.From<int>([Error.Validation(), Error.Validation()]);
```

```cs
public ErrorOr<int> GetValue()
{
    return ErrorOrFactory.From(5);
}
```

```cs
public ErrorOr<int> SingleErrorToErrorOr()
{
    return ErrorOrFactory.From<int>(Error.Unexpected());
}
```

```cs
public ErrorOr<int> MultipleErrorsToErrorOr()
{
    return ErrorOrFactory.From([
        Error.Validation(description: "Invalid Name"),
        Error.Validation(description: "Invalid Last Name")
    ]);
}
```

## Using The `ToErrorOr` Extension Method

```cs
ErrorOr<int> result = 5.ToErrorOr();
ErrorOr<int> result = Error.Unexpected().ToErrorOr<int>();
ErrorOr<int> result = new[] { Error.Validation(), Error.Validation() }.ToErrorOr<int>();
```

# Properties

## `IsError`

```cs
ErrorOr<int> result = User.Create();

if (result.IsError)
{
    // the result contains one or more errors
}
```

## `Value`

```cs
ErrorOr<int> result = User.Create();

if (!result.IsError) // the result contains a value
{
    Console.WriteLine(result.Value);
}
```

## `Errors`

```cs
ErrorOr<int> result = User.Create();

if (result.IsError)
{
    result.Errors // contains the list of errors that occurred
        .ForEach(error => Console.WriteLine(error.Description));
}
```

## `FirstError`

```cs
ErrorOr<int> result = User.Create();

if (result.IsError)
{
    var firstError = result.FirstError; // only the first error that occurred
    Console.WriteLine(firstError == result.Errors[0]); // true
}
```

## `ErrorsOrEmptyList`

```cs
ErrorOr<int> result = User.Create();

if (result.IsError)
{
    result.ErrorsOrEmptyList // List<Error> { /* one or more errors */  }
    return;
}

result.ErrorsOrEmptyList // List<Error> { }
```

# Methods

## `Match`

The `Match` method receives two functions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

### `Match`

```cs
string foo = result.Match(
    value => value,
    errors => $"{errors.Count} errors occurred.");
```

### `MatchAsync`

```cs
string foo = await result.MatchAsync(
    value => Task.FromResult(value),
    errors => Task.FromResult($"{errors.Count} errors occurred."));
```

### `MatchFirst`

The `MatchFirst` method receives two functions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

Unlike `Match`, if the state is error, `MatchFirst`'s `onError` function receives only the first error that occurred, not the entire list of errors.

```cs
string foo = result.MatchFirst(
    value => value,
    firstError => firstError.Description);
```

### `MatchFirstAsync`

```cs
string foo = await result.MatchFirstAsync(
    value => Task.FromResult(value),
    firstError => Task.FromResult(firstError.Description));
```

## `Switch`

The `Switch` method receives two actions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

### `Switch`

```cs
result.Switch(
    value => Console.WriteLine(value),
    errors => Console.WriteLine($"{errors.Count} errors occurred."));
```

### `SwitchAsync`

```cs
await result.SwitchAsync(
    value => { Console.WriteLine(value); return Task.CompletedTask; },
    errors => { Console.WriteLine($"{errors.Count} errors occurred."); return Task.CompletedTask; });
```

### `SwitchFirst`

The `SwitchFirst` method receives two actions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

Unlike `Switch`, if the state is error, `SwitchFirst`'s `onError` function receives only the first error that occurred, not the entire list of errors.

```cs
result.SwitchFirst(
    value => Console.WriteLine(value),
    firstError => Console.WriteLine(firstError.Description));
```

### `SwitchFirstAsync`

```cs
await result.SwitchFirstAsync(
    value => { Console.WriteLine(value); return Task.CompletedTask; },
    firstError => { Console.WriteLine(firstError.Description); return Task.CompletedTask; });
```

## `Then`

### `Then`

`Then` receives a function, and invokes it only if the result is not an error.

```cs
ErrorOr<int> foo = result
    .Then(val => val * 2);
```

Multiple `Then` methods can be chained together.

```cs
ErrorOr<string> foo = result
    .Then(val => val * 2)
    .Then(val => $"The result is {val}");
```

If any of the methods return an error, the chain will break and the errors will be returned.

```cs
ErrorOr<int> Foo() => Error.Unexpected();

ErrorOr<string> foo = result
    .Then(val => val * 2)
    .Then(_ => GetAnError())
    .Then(val => $"The result is {val}") // this function will not be invoked
    .Then(val => $"The result is {val}"); // this function will not be invoked
```

### `ThenAsync`

`ThenAsync` receives an asynchronous function, and invokes it only if the result is not an error.

```cs
ErrorOr<string> foo = await result
    .ThenAsync(val => DoSomethingAsync(val))
    .ThenAsync(val => DoSomethingElseAsync($"The result is {val}"));
```

### `ThenDo` and `ThenDoAsync`

`ThenDo` and `ThenDoAsync` are similar to `Then` and `ThenAsync`, but instead of invoking a function that returns a value, they invoke an action.

```cs
ErrorOr<string> foo = result
    .ThenDo(val => Console.WriteLine(val))
    .ThenDo(val => Console.WriteLine($"The result is {val}"));
```

```cs
ErrorOr<string> foo = await result
    .ThenDoAsync(val => Task.Delay(val))
    .ThenDo(val => Console.WriteLine($"Finsihed waiting {val} seconds."))
    .ThenDoAsync(val => Task.FromResult(val * 2))
    .ThenDo(val => $"The result is {val}");
```

### Mixing `Then`, `ThenDo`, `ThenAsync`, `ThenDoAsync`

You can mix and match `Then`, `ThenDo`, `ThenAsync`, `ThenDoAsync` methods.

```cs
ErrorOr<string> foo = await result
    .ThenDoAsync(val => Task.Delay(val))
    .Then(val => val * 2)
    .ThenAsync(val => DoSomethingAsync(val))
    .ThenDo(val => Console.WriteLine($"Finsihed waiting {val} seconds."))
    .ThenAsync(val => Task.FromResult(val * 2))
    .Then(val => $"The result is {val}");
```

## `FailIf`

`FailIf` receives a predicate and an error. If the predicate is true, `FailIf` will return the error. Otherwise, it will return the value of the result.

```cs
ErrorOr<int> foo = result
    .FailIf(val => val > 2, Error.Validation(description: $"{val} is too big"));
```

Once an error is returned, the chain will break and the error will be returned.

```cs
var result = "2".ToErrorOr()
    .Then(int.Parse) // 2
    .FailIf(val => val > 1, Error.Validation(description: $"{val} is too big") // validation error
    .Then(num => num * 2) // this function will not be invoked
    .Then(num => num * 2) // this function will not be invoked
```

## `Else`

`Else` receives a value or a function. If the result is an error, `Else` will return the value or invoke the function. Otherwise, it will return the value of the result.

### `Else`

```cs
ErrorOr<string> foo = result
    .Else("fallback value");
```

```cs
ErrorOr<string> foo = result
    .Else(errors => $"{errors.Count} errors occurred.");
```

### `ElseAsync`

```cs
ErrorOr<string> foo = await result
    .ElseAsync(Task.FromResult("fallback value"));
```

```cs
ErrorOr<string> foo = await result
    .ElseAsync(errors => Task.FromResult($"{errors.Count} errors occurred."));
```

### `ElseDo` and `ElseDoAsync`

`ElseDo` and `ElseDoAsync` are similar to `Else` and `ElseAsync`, but instead of invoking a function that returns an error, they invoke an action.

```cs
ErrorOr<string> foo = result
    .Else(errors => Error.Unexpected())
    .ElseDo(error => Console.WriteLine(error.FirstError.Description));
```

```cs
ErrorOr<string> foo = await result
    .ElseDoAsync(HandleErrorAsync);
```

# Mixing Features (`Then`, `FailIf`, `Else`, `Switch`, `Match`)

You can mix `Then`, `FailIf`, `Else`, `Switch` and `Match` methods together.

```cs
ErrorOr<string> foo = await result
    .ThenDoAsync(val => Task.Delay(val))
    .FailIf(val => val > 2, Error.Validation(description: $"{val} is too big"))
    .ThenDo(val => Console.WriteLine($"Finished waiting {val} seconds."))
    .ThenAsync(val => Task.FromResult(val * 2))
    .Then(val => $"The result is {val}")
    .Else(errors => Error.Unexpected())
    .MatchFirst(
        value => value,
        firstError => $"An error occurred: {firstError.Description}");
```

# Error Types

Each `Error` instance has a `Type` property, which is an enum value that represents the type of the error.

## Built in error types

The following error types are built in:

```cs
public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
}
```

Each error type has a static method that creates an error of that type. For example:

```cs
var error = Error.NotFound();
```

optionally, you can pass a code, description and metadata to the error:

```cs
var error = Error.Unexpected(
    code: "User.ShouldNeverHappen",
    description: "A user error that should never happen",
    metadata: new Dictionary<string, object>
    {
        { "user", user },
    });
```

The `ErrorType` enum is a good way to categorize errors.

## Custom error types

You can create your own error types if you would like to categorize your errors differently.

A custom error type can be created with the `Custom` static method

```cs
public static class MyErrorTypes
{
    const int ShouldNeverHappen = 12;
}

var error = Error.Custom(
    type: MyErrorTypes.ShouldNeverHappen,
    code: "User.ShouldNeverHappen",
    description: "A user error that should never happen");
```

You can use the `Error.NumericType` method to retrieve the numeric type of the error.

```cs
var errorMessage = Error.NumericType switch
{
    MyErrorType.ShouldNeverHappen => "Consider replacing dev team",
    _ => "An unknown error occurred.",
};
```

# Built in result types (`Result.Success`, ..)

There are a few built in result types:

```cs
ErrorOr<Success> result = Result.Success;
ErrorOr<Created> result = Result.Created;
ErrorOr<Updated> result = Result.Updated;
ErrorOr<Deleted> result = Result.Deleted;
```

Which can be used as following

```cs
ErrorOr<Deleted> DeleteUser(Guid id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user is null)
    {
        return Error.NotFound(description: "User not found.");
    }

    await _userRepository.DeleteAsync(user);
    return Result.Deleted;
}
```

# Organizing Errors

A nice approach, is creating a static class with the expected errors. For example:

```cs
public static partial class DivisionErrors
{
    public static Error CannotDivideByZero = Error.Unexpected(
        code: "Division.CannotDivideByZero",
        description: "Cannot divide by zero.");
}
```

Which can later be used as following 👇

```cs
public ErrorOr<float> Divide(int a, int b)
{
    if (b == 0)
    {
        return DivisionErrors.CannotDivideByZero;
    }

    return a / b;
}
```

# [Mediator](https://github.com/jbogard/MediatR) + [FluentValidation](https://github.com/FluentValidation/FluentValidation) + `ErrorOr` 🤝

A common approach when using `MediatR` is to use `FluentValidation` to validate the request before it reaches the handler.

Usually, the validation is done using a `Behavior` that throws an exception if the request is invalid.

Using `ErrorOr`, we can create a `Behavior` that returns an error instead of throwing an exception.

This plays nicely when the project uses `ErrorOr`, as the layer invoking the `Mediator`, similar to other components in the project, simply receives an `ErrorOr` and can handle it accordingly.

Here is an example of a `Behavior` that validates the request and returns an error if it's invalid 👇

```cs
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        return (dynamic)errors;
    }
}
```

# Contribution 🤲

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# Credits 🙏

- [OneOf](https://github.com/mcintyre321/OneOf/tree/master/OneOf) - An awesome library which provides F# style discriminated unions behavior for C#

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/mantinband/error-or/blob/main/LICENSE) license.
