Watchstander
============

a toolkit for bosun

  * introduction
  * a warning
  * getting started
  * running the tests
  * api reference

introduction
------------

Watchstander is a toolkit for interacting with OpenTSDB generally, and
Bosun specifically.  Bosun is a monitoring and alerting system built on
top of OpenTSDB, see [bosun.org](http://bosun.org).  The need for a
flexible, efficient client has driven the development of this library.

Watchstander is built to be hackable so you can use it in whatever way
you'd like to get your data into Bosun.  There are three main levels
of usage you may go for.

You might just use it as a simple wrapper for the JSON API, to elide
the mechanics of sending requests to Bosun while maintaining the best
performance possible.  All you need is to implement the `IDataPoint`
interface, and you can push your data points to Bosun with a simple
`api.Put(dataPoint)`.  And likewise with metadata.

The next level of usage is to take advantage of the Bosun schema
abstraction.  By providing in advance the `IMetric`s and `ITimeSeries`
that you wish to use, you can ensure that all data points conform to
the expected schema.  You can specify this schema in a configuration
section of code or load it dynamically from an established Bosun server.

Complete adoption means using the highest-level API, the porcelain layer
constructed out of the building blocks of the plumbing layer API.  It
provides the fully-featured abstractions needed to provide clients with
effortless metrics servicing, including client-side aggregation, metric
name and tag limiting, and worker management.

a warning
---------

At the moment, this library is under active development.  Some important
things may very well be broken.  *This document is intended to be
aspirational* -- it is *proscriptive* and not necessarily *descriptive*.
As always, the only true documentation is the code.

getting started
---------------

While there is a loose framework of porcelain available, much of the
most useful bits have yet to be implemented.  You're mostly stuck with
an assemblage of the basic bits at the moment.  In case you feel like
diving into the raw plumbing, the `Api` class will be your friend.  If
you want to work with metadata you might check out the `MetadataFactory`
or the `SchemaLoader`.  In any case you'll want to be familiar with the
interfaces in `Common`.

Eventually we'll want to be able to do something like this to get started:

```csharp
using Watchstander;

var options = new CollectorOptions(new Uri(myBosunUrl));
var collector = Bosun.Collector(options)
    .WithName("application")
    .WithTag("host", hostname)
    .WithSchema(schema => {
        schema
          .GetMetric("foo", Rate.Counter, "times fooed")
          .Description = "The number of times we fooed something.";
    });

var myCounter = collector.GetMetric("foo");
myCounter.Increment();
```

Which of course, would report the metric `application.foo` to the Bosun
instance at `myBosunUrl` with the tag key `"host"` having the value of
`hostname`.  It would also send metadata for the metric `application.foo`,
including the description, rate, and units.

Roughly equivalent using the currently implemented classes would be:

```csharp
using Watchstander;

var options = new CollectorOptions(new Uri(myBosunUrl)));

var collector = Bosun.Collector(options)
    .WithName("application")
    .WithTag("host", hostname);

var myCounter = collector.GetMetric<long>("foo");
myCounter.Record(1);
```

Which would record your data point but not the metadata.  To do that,
you'd need the plumbing.

```csharp
using Watchstander.Common;
using Watchstander.Plumbing;

myCounter.Description = "The number of times we fooed something.";

var rate = myCounter.GetRateMetadata();
var unit = myCounter.GetUnitMetadata();
var desc = myCounter.GetDescriptionMetadata();

var metadata = new List<IMetadata>{ rate, unit, desc };

// assuming you have a handle on the api object inside the above collector
api.PutMetadata(metadata);
```

Which works, but obviously isn't ideal.

running the tests
-----------------

The integration tests require a Bosun instance with the default
configuration available on localhost (that is, we can make HTTP requests
to `localhost:8070/api`).  If you need another setup to run the tests
locally you are welcome to introduce some indirection here.

api reference
-------------

*you'd like that, wouldn't you?*

##### ╭╮☲☲☲╭╮ #####
