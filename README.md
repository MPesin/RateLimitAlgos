# DoS Home Assignment 
This is an ASP.Net (`C#`) project that implements two simple rate limiters for requests:
1. A ___static___ rate limiter based on a simple Fixed Window algorithm.
1. A ___dynamic___ rate limiter based on a Sliding Window Log algorithm.

## Routing
As required by the assignment, there are two endpoints the the service listens to:
1. For the ___static___ limiter: `http://localhost:8080/StaticWindow?clientId=[id]`
1. For the ___dynamic___ limiter: `http://localhost:8080/DynamicWindow?clientId=[id]`

## Testing

The `Tests` project contain test for the ___static___ algorithm. 
I used ___TDD___ when developing this answer, so each test represents a different property / function of the algorithm. 

## Personal Info

__Email__: mikelpesin@gmail.com

__Mobile__: 054-4929277

[Linkedin](https://www.linkedin.com/in/michaelpesin87/)