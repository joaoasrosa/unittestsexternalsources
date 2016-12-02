var Assemblies = React.createClass({
    getInitialState: function () {
        return { data: this.props.initialData };
    },
    render: function () {
        var assembliesComponent;
        if (this.state.data && this.state.data.length > 0) {
            assembliesComponent = <AssembliesTable data={this.state.data } />;
        } else {
            assembliesComponent = <AssembliesWarning />;
        }

        return (assembliesComponent);
    }
});

var AssembliesWarning = React.createClass({
    render: function () {
        return (
            <div className="alert alert-info" role="alert"><strong>There is no assemblies in the system.</strong> Please use the Upload button to start.</div>
            );
    }
});

var AssembliesTable = React.createClass({
    render: function () {
        return (
           <table className="table table-striped">
               <thead>
               <tr>
                   <th>Name</th>
                   <th>Version</th>
                   <th>Path</th>
               </tr>
               </thead>
               <AssembliesTableBody data={ this.props.data } />
           </table>
       );
    }
});

var AssembliesTableBody = React.createClass({
    render: function () {
        const assemblyRow = this.props.data.map(function (assembly) {
            return (
                <tr key={assembly.Name}>
                    <td><a href={'/Assembly/' + assembly.Name + '/Class' }>{assembly.Name}</a></td>
                    <td>{assembly.Version}</td>
                    <td>{assembly.Path}</td>
                </tr>
            );
        });
        return (
        <tbody>
            {assemblyRow}
        </tbody>);
    }
});

ReactDOM.render(<Assemblies initialData={initialData } />, document.getElementById('assemblies'));