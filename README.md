Typesafe Url
============

A very basic approach to generating urls in asp.net with compile time typesafety. Under the hood it consumes expressions and hands the data over to asp.nets UrlHelper to generate urls that match your routes.

### Setup
Typical integration for razor support is to extend the asp.net provided base view model and attach this to it. For c# support you can attach it to your base controller.

### Example usage
Say you had a route handler for /{controller}/{action}/{id} with id being optional
Then assume a controller that enables user management, methods like index, view and edit. To generate urls for such do:
```csharp
// action signature of: public ActionResult View();
typesafeUrl.Url<UserController>(_ => _.Index())
// returns /User/Index

// action signature of: public ActionResult Edit(int id);
typesafeUrl.Url<UserController>(_ => _.Edit(1))
// returns /User/Edit/1
```

Multiple url parameters?
```csharp
// action signature of: public ActionResult View(int id, bool showRelatives)
typesafeUrl.Url<UserController>(_ => _.View(1,true))
// returns /User/View/1?showRelatives=true
```