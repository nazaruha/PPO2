import { Link, useNavigate, useParams } from "react-router-dom";
import { IManufacturerEditItem } from "../types";
import { useFormik } from "formik";
import { ManufacturerEditSchema } from "../validation";
import { useEffect } from "react";
import http from "../../../../http_common";
import classNames from "classnames";

const ManufacturerEditPage = () => {
    const navigate = useNavigate();

    const { id } = useParams();
    const init: IManufacturerEditItem = {
        id: Number(id),
        name: ""
    };

    const onEditSubmit = (values: IManufacturerEditItem) => {
        console.log('VALUES', values);
        http.put(`api/manufacturer/update/${id}`, values)
            .then(resp => {
                console.log("RESP", resp);
                navigate("..");
            })
            .catch(err => {
                const { data } = err.response;
                console.log("ERR", data);
                setFieldError("name", data.message);
            })
    }

    const formik = useFormik({
        initialValues: init,
        onSubmit: onEditSubmit,
        validationSchema: ManufacturerEditSchema
    })

    const { errors, values, touched, setFieldValue, setFieldError, handleChange, handleSubmit } = formik;

    useEffect(() => {
        http.get(`api/manufacturer/${id}`)
            .then(resp => {
                console.log("RESP", resp);
                const { data } = resp;
                setFieldValue("name", data.payload.name);
            })
            .catch(err => {
                console.log("ERR", err);
            })
    }, []);

    return (
        <>
            <h1 className="text-center">Редагування виробника</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3">
                    <div className="mb-3">
                        <label htmlFor="name" className="form-label">Назва <i className="fa fa-snowflake-o text-danger fa-2xs" aria-hidden="true"></i></label>
                        <input id="name" name="name" className={classNames(
                            "form-control",
                            { "is-invalid": errors.name && touched.name }
                        )} value={values.name} onChange={handleChange} placeholder="Введіть пошту" />
                        {errors.name && touched.name && (
                            <div className="invalid-feedback">
                                {errors.name}
                            </div>
                        )}
                    </div>

                    <div className="container d-flex flex-row justify-content-center gap-3">
                        <button type="submit" className="btn btn-primary">Змінити</button>
                        <Link to="../" className="btn btn-warning">Відхілити</Link>
                    </div>
                </div>
            </form>
        </>
    )
}

export default ManufacturerEditPage;