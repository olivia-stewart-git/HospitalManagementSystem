# Read me

Explaining the architecture

For this project I wanted to approach it as trying to develop a framework for developing console applications in a MVC pattern similar to other frameworks.

Input is run on a separate thread. I wanted to do this so I could asynchronously render/defer input however I did not have times.
The app works by having views, which are made of individual controls such as buttons, tables etc.
The views function like a single form/page of the application.

The controls work as a tree where they have reference to their parents and children to allow for inheritance of render properties. However due to limitations of C# console there is not a ton one can actually do with this.
The whole project uses EntityFramework for ORM. Dependency injection is also configured.

For ease of use, sqllite configuration is uses in deployed version for testing.
https://github.com/cread134/HospitalManagementSystem
