var Methods = React.createClass({
    getInitialState: function () {
        return { data: this.props.initialData };
    },
    render: function () {
        var methodsComponent;
        if (this.state.data && this.state.data.length > 0) {
            methodsComponent = <MethodsTable data={this.state.data } />;
        } else {
            methodsComponent = <MethodsWarning />;
        }

        return (methodsComponent);
    }
});

var MethodsWarning = React.createClass({
    render: function () {
        return (
            <div className="alert alert-info" role="alert"><strong>There is no methods in the class.</strong> Please use the Upload button to in the Assembly page.</div>
            );
    }
});

var MethodsTable = React.createClass({
    render: function () {
        return (
           <table className="table table-striped">
               <thead>
               <tr>
                   <th>Name</th>
               </tr>
               </thead>
               <MethodsTableBody data={ this.props.data } />
           </table>
       );
    }
});

var MethodsTableBody = React.createClass({
    render: function () {
        const methodRow = this.props.data.map(function (methodClass) {
            return (
                <tr key={methodClass.Name}>
                    <td>{methodClass.Name}</td>
                </tr>
            );
        });
        return (
        <tbody>
            {methodRow}
        </tbody>);
    }
});

ReactDOM.render(<Methods initialData={initialData} />, document.getElementById('methods'));