var Parameters = React.createClass({
    getInitialState: function () {
        return { data: this.props.initialData };
    },
    render: function () {
        var parametersComponent;
        if (this.state.data && this.state.data.length > 0) {
            parametersComponent = <ParametersTable data={this.state.data } />;
        } else {
            parametersComponent = <ParametersWarning />;
        }

        return (parametersComponent);
    }
});

var ParametersWarning = React.createClass({
    render: function () {
        return (
            <div className="alert alert-info" role="alert"><strong>There is no parameters in the method.</strong> Please use the Upload button to in the Assembly page.</div>
            );
    }
});

var ParametersTable = React.createClass({
    render: function () {
        return (
           <table className="table table-striped">
               <thead>
               <tr>
                   <th>Name</th>
                   <th>Type</th>
               </tr>
               </thead>
               <ParametersTableBody data={ this.props.data } />
           </table>
       );
    }
});

var ParametersTableBody = React.createClass({
    render: function () {
        const parameterRow = this.props.data.map(function (parameterClass) {
            return (
                <tr key={parameterClass.Name}>
                    <td>{parameterClass.Name}</td>
                    <td>{parameterClass.Type}</td>
                </tr>
            );
        });
        return (
        <tbody>
            {parameterRow}
        </tbody>);
    }
});

ReactDOM.render(<Parameters initialData={initialData } />, document.getElementById('parameters'));