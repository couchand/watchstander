Watchstander
============

a toolkit for bosun

  * introduction
  * a warning
  * getting started
  * api reference

introduction
------------

Watchstander is a toolkit for interacting with OpenTSDB generally, and
Bosun specifically.  Bosun is a monitoring dashboard built on top of
OpenTSDB, see [bosun.org](http://bosun.org).  The need for a flexible,
efficient client has driven the development of this library.

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
which will be constructed when the plumbing layer API is finalized.  It
will provide the client-facing abstractions around metrics, including
client-side aggregation, metric name and tag limiting, and worker
management.

a warning
---------

At the moment, this library is under active development.  Some important
things may very well be broken.  *This document is intended to be
aspirational* -- it is *proscriptive* and not necessarily *descriptive*.
As always, the only true documentation is the code.

getting started
---------------

There's no porcelain yet, so you're stuck with the raw plumbing.  The
`Api` class will be your friend.  If you want to work with metadata you
might check out the `MetadataFactory` or the `SchemaLoader`.  In any
case you'll want to be familiar with the interfaces in `Common`.

Eventually we'll want to be able to do something like this to get started:

    using Watchstander;

    var options = new CollectorOptions(new Uri(myBosunUrl));
    var collector = new Collector(options)
        .WithNamePrefix("application")
        .WithTag("host", hostname);

    var myCounter = collector.Register(new Counter("foo", "times fooed"));
    myCounter.Description = "The number of times we fooed something.";

    myCounter.Increment();

Which of course, would report the metric `application.foo` to the Bosun
instance at `myBosunUrl` with the tag key `"host"` having the value of
`hostname`.  It would also send metadata for the metric `application.foo`,
including the description, rate, and units.

Roughly equivalent using the currently implemented plumbing would be:

    using Watchstander.Common;
    using Watchstander.Plumbing;
    using Watchstander.Utilities;

    var options = new ApiOptions(new Uri(myBosunUrl), new SerializerOptions());
    var api = new Api(options);

    var tags = new Dictionary<string, string>();
    tags["host"] = hostname;

    var dataPoint = new WatchstanderTests.DataTest(
      "application.foo", DateTime.UtcNow, 1, tags.AsReadOnly()
    );

    var rate = new WatchstanderTests.MetadataTests(
      "application.foo", null, "rate", "counter"
    );

    var unit = new WatchstanderTests.MetadataTests(
      "application.foo", null, "unit", "times fooed"
    );

    var desc = new WatchstanderTests.MetadataTests(
      "application.foo", null, "desc", "The number of times we fooed something."
    );

    var metadata = new List<IMetadata>{ rate, unit, desc};

    api.Put(dataPoint);
    api.PutMetadata(metadata);

Which works, but obviously isn't ideal.

api reference
-------------

*you'd like that, wouldn't you?*

##### ╭╮☲☲☲╭╮ #####
