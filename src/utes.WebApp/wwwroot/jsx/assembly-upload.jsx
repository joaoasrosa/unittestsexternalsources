var AssemblyForm = React.createClass({
    getInitialState: function () {
        return {
            assemblyFile: null,
            assemblyFileName: '',
            errorHeading: '',
            errorMessage: '',
            uploadDone: false
        };
    },
    handleSubmit: function (e) {
        e.preventDefault();
        if (!this.state.assemblyFile) {
            this.setState({
                assemblyFile: null,
                assemblyFileName: ''
            });
            return;
        }

        const formData = new FormData();
        formData.append('assemblyFile', this.state.assemblyFile);
        formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

        $.ajax({
            url: this.props.url,
            dataType: 'json',
            type: 'POST',
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: formData,
            success: function (response) {
                if (response.sucess) {
                    window.location = response.redirectTo;
                } else {
                    this.setState({
                        assemblyFile: null,
                        assemblyFileName: '',
                        errorHeading: response.errorHeading,
                        errorMessage: response.errorMessage
                    });
                }
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());

                this.setState({
                    assemblyFile: null,
                    assemblyFileName: '',
                    errorHeading: 'An internal error occur.',
                    errorMessage: 'Please review the logs.'
                });
            }.bind(this)
        });
    },
    handleAssemblyFileChange: function (e) {
        if (!e.target.value || !e.target.value.endsWith('.dll')) {
            this.setState({
                assemblyFile: null,
                assemblyFileName: ''
            });
            return;
        }
        this.setState({
            assemblyFile: e.target.files[0],
            assemblyFileName: e.target.value
        });
    },
    render: function () {
        return (
            <form className="form-horizontal" encType="multipart/form-data" onSubmit={this.handleSubmit}>
                <AssemblyUploadWarning errorHeading={this.state.errorHeading} errorMessage={this.state.errorMessage} />
                <div className="form-group">
                    <label htmlFor="assemblyFile" className="col-sm-2 control-label">Assembly</label>
                    <div className="col-sm-10">
                        <input type="file" id="assemblyFile" accept=".dll" required="required"
                               value={this.state.assemblyFileName}
                               onChange={this.handleAssemblyFileChange} />
                    </div>
                </div>
                <div className="form-group">
                    <div className="col-sm-offset-2 col-sm-10">
                        <button type="submit" className="btn btn-primary">Upload</button>
                    </div>
                </div>
            </form>
        );
    }
});

var AssemblyUploadWarning = React.createClass({
    render: function () {
        var assemblyUploadComponent = <div></div>;
        if (this.props.errorHeading) {
            assemblyUploadComponent = (<div className="alert alert-warning" role="alert"><strong>{this.props.errorHeading}</strong> {this.props.errorMessage}</div>);
        }
        return (assemblyUploadComponent);
    }
});

ReactDOM.render(<AssemblyForm url="/Assembly/Upload" />, document.getElementById('assemblyForm'));