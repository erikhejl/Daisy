Daisy is a <a href="http://martinfowler.com/bliki/BusinessReadableDSL.html">Business writeable</a> domain sepecific lanaguage that lets you describe business rules without detailing how they are implemented.  Daisy starts domain agnostic, but allows domain experts and software developers to create the domain together. The domain experts write rules, then the software developers write the implementation of those rules.

An example syntax is:
Any user
  Is Active
  Has Transaction
    Balance is less than 0
    OR Balance is greater than 1000

Daisy handles the boolean algebra and linking statements to functions.  Domain experts handle writing business rules in a language that not only makes sense to them, but they invent. Software engineers handle implementing each statement in code that their software understands.
