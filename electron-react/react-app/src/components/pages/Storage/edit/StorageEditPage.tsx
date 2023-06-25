import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { IManufacturerItem, IManufacturerServiceResponse } from "../../Manufacturer/types";
import { IProductItem } from "../../Product/types";
import { IStorageEditItem, IStorageItem } from "../types";
import { useFormik } from "formik";
import { StorageEditSchema } from "../validation";
import http from "../../../../http_common";
import classNames from "classnames";

const StorageEditPage = () => {
    const { productId, manufacturerId, projectId } = useParams();

    const navigate = useNavigate();

    const [productName, setProductName] = useState<string>("");
    const [manufacturerName, setManufecturerName] = useState<string>("");

    const init: IStorageEditItem = {
        productId: 0,
        projectId: Number(projectId),
        price: 0,
        count: 0,
        description: "",
        expireDate: new Date()
    }

    const onEditSubmit = (values: IStorageEditItem) => {
        console.log("VALUES", values);

        http.put("api/storage/update", values)
            .then(resp => {
                navigate("..");
            }).catch(err => {
                console.log("ERR PUT DATA", err);
            })

    }

    const formik = useFormik({
        initialValues: init, // данні які передаєм (початкові налаштування для полів)
        onSubmit: onEditSubmit, // метод, який спрацює при submit форми і коли усі дані у форміку валідні
        validationSchema: StorageEditSchema // схема валідації даних
    })

    const { errors, values, touched, setFieldValue, setFieldError, handleChange, handleSubmit } = formik;

    useEffect(() => {
        http.get<IStorageItem>(`api/storage/product-${productId}/project-${projectId}`)
            .then(resp => {
                console.log("GET STORAGE ITEM", resp);
                setFieldValue("manufacturerId", resp.data.product.manufacturerId);
                setFieldValue("productId", resp.data.productId);
                setFieldValue("description", resp.data.description);
                setFieldValue("price", resp.data.price);
                setFieldValue("count", resp.data.count);
                setFieldValue('expireDate', resp.data.expireDate);
            }).catch(err => {
                console.log("ERR GETTING STORAGE ITEM", err);
            })

        http.get(`api/manufacturer/${manufacturerId}`)
            .then(resp => {
                console.log("GET MAN BY ID", resp)
                setManufecturerName(resp.data.payload.name);
            }).catch(err => {
                console.log("ERR GET MAN NAME", err);
            })

        http.get(`api/product/${productId}`)
            .then(resp => {
                console.log("GET PROD BY ID", resp)
                setProductName(resp.data.payload.name);
            }).catch(err => {
                console.log("ERR GET PROD NAME", err);
            })
    }, [])

    return (
        <>
            <h1 className="text-center">Сховище - редагування продукту</h1>
            <h3 className="text-center">{productName} | {manufacturerName}</h3>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3">
                    <label htmlFor="description" className="form-label">Опис</label>
                    <input type="text" id="description" name="description" className={classNames(
                        "form-control",
                        { "is-invalid": errors.description && touched.description }
                    )} value={values.description} onChange={handleChange} placeholder="Введіть опис" />
                    {errors.description && touched.description && (
                        <div className="invalid-feedback">
                            {errors.description}
                        </div>
                    )}
                </div>
                <div className="mb-3">
                    <label htmlFor="price" className="form-label">Ціна</label>
                    <input type="number" id="price" name="price" className={classNames(
                        "form-control",
                        { "is-invalid": errors.price && touched.price }
                    )} value={values.price} onChange={handleChange} placeholder="Введіть ціну" />
                    {errors.price && touched.price && (
                        <div className="invalid-feedback">
                            {errors.price}
                        </div>
                    )}
                </div>
                <div className="mb-3">
                    <label htmlFor="count" className="form-label">Кількість</label>
                    <input type="number" id="count" name="count" className={classNames(
                        "form-control",
                        { "is-invalid": errors.count && touched.count }
                    )} value={values.count} onChange={handleChange} placeholder="Введіть кількість" />
                    {errors.count && touched.count && (
                        <div className="invalid-feedback">
                            {errors.count}
                        </div>
                    )}
                </div>

                <div className="mb-3">
                    <label htmlFor="expireDate" className="form-label">Термін придатності</label>
                    <input
                        type="datetime-local"
                        className={classNames(
                            "form-control",
                            { "is-invalid": errors.expireDate && touched.expireDate }
                        )}
                        id="expireDate"
                        name="expireDate"
                        onChange={handleChange}
                        value={values.expireDate.toLocaleString()}
                    />
                    {errors.expireDate && touched.expireDate && (
                        <div className="invalid-feedback">
                            {errors.expireDate}
                        </div>
                    )}
                </div>

                <div className="container d-flex flex-row justify-content-center gap-3">
                    <button type="submit" className="btn btn-primary">Редагувати</button>
                    <Link to="../" className="btn btn-warning">Відхілити</Link>
                </div>
            </form>
        </>
    )
}

export default StorageEditPage;