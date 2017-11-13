var CommentBox = React.createClass({
  render: function() {
    return (
      <div className="commentBox">
        Hello, world! I am a CommentBox.
      </div>
    );
  }
});

var solutionType = React.createClass({
    render: function () {
        return (
            <div class="tile tile-long">
                <img class="tile-illustration" data-bind="attr: { src: illustrationUrl }" src="/content/styles/img/RemoteMonitoring.jpg"></img>
                <div class="panel-body tile-full-height">
                    <h2 data-bind="text: name">Remote monitoring</h2>
                    <div data-bind="text: description">Connect and monitor your devices to analyze untapped data and improve business outcomes by automating processes.</div>
                </div>
                <a class="btn btn-primary" data-bind="attr: { href: '#solutions/types/'+id }, traceClick: '[{0}] Solution Types > Select'.format(id), text: Resources.ButtonLabelSelectSolution" href="#solutions/types/RM">Select</a>
            </div>
        );
    }
});

ReactDOM.render(
  <CommentBox />,
  document.getElementById('content')
);