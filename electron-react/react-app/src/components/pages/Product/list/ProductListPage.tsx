import { Link, useSearchParams } from "react-router-dom";
import { IProductSearch, IProductSearchResult } from "../types";
import { useEffect, useState } from "react";
import { useFormik } from "formik";
import http from "../../../../http_common";
import classNames from "classnames";
import ModalDelete2 from "../../../common/ModalDelete2";

const ProductListPage = () => {
    const [searchParams, setSearchParams] = useSearchParams();

    const [search, setSearch] = useState<IProductSearch>({
        name: searchParams.get("name") || "", //3
        manufacturerName: searchParams.get("manufacturerName") || "", //3
        page: searchParams.get("page") || 1,
    });

    const onSubmitSearch = (values: IProductSearch) => {
        console.log("Проводимо пошук", values);
        setSearch(values);
    }

    const formik = useFormik({
        initialValues: search,
        onSubmit: onSubmitSearch
    });

    const { values, touched, errors, handleSubmit, handleChange } = formik;

    const [searchResult, setSearchResult] = useState<IProductSearchResult>({
        products: [],
        pages: 0,
        currentPage: 0,
        total: 0,
    });

    useEffect(() => {
        console.log(search);
        http.get<IProductSearchResult>(`api/product/search`, {
            params: search
        })
            .then(resp => {
                const { data } = resp;
                console.log("Server response", data);
                setSearchResult(data);
            }).catch(err => {
                console.log("ERR", err);
            })
    }, [search]);

    const { products, pages, currentPage } = searchResult;

    const buttons = [];
    for (let i = 1; i <= pages; i++) {
        buttons.push(i);
    }

    const pagination = buttons.map(x => {
        return (
            <li className={classNames(
                "page-item",
                { "active": x === currentPage }
            )} key={x}>
                <Link className="page-link" to={`?page=${x}`} onClick={() => setSearch({ ...search, page: x })}>
                    {x}
                </Link>
            </li>
        );
    })

    const onDeleteProduct = async (productId: number, manufacturerId: number) => {
        try {
            await http.delete(`api/product/delete/product-${productId}/manufacturer-${manufacturerId}`);
            setSearch({ ...search, page: 1 });
        } catch (err) {
            console.log("ERR DELETE", err);
        }
    }

    const tableViewList = products.map((item) => {
        return (
            <tr key={item.id} className="text-center">
                <th scope="row">{item.id}</th>
                <td>{item.name}</td>
                <td>{item.manufacturerName}</td>
                <td style={{ display: 'flex', justifyContent: 'center' }}>
                    <Link to={`edit/${item.id}/${item.manufacturerId}`} className="btn btn-warning"
                    >
                        Змінити <i className="fa fa-pencil" aria-hidden="true"></i>
                    </Link>
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    <ModalDelete2
                        childId={item.id}
                        parentId={item.manufacturerId}
                        text={`Ви дійсно бажаєте видалити Продукт '${item.name}' з даного виробника '${item.manufacturerName}'?`}
                        deleteFunc={onDeleteProduct}
                    />
                </td>
            </tr>
        )
    })

    return (
        <>
            <h1 className="text-center">Список Продуктів</h1>

            <form onSubmit={handleSubmit} className="mb-5">
                <div className="row">
                    <div className="col-md-5">
                        <label htmlFor="name" className="form-label">Продукт</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={values.name}
                            placeholder="Пошук по назві продукта"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.name && touched.name }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-5">
                        <label htmlFor="manufacturerName" className="form-label">Виробник</label>
                        <input
                            type="text"
                            id="manufacturerName"
                            name="manufacturerName"
                            value={values.manufacturerName}
                            placeholder="Пошук по назві виробника"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.manufacturerName && touched.manufacturerName }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2" style={{ display: 'flex', alignItems: 'end' }}>
                        <button type="submit" className="btn btn-primary">Пошук</button>
                    </div>
                </div>
            </form>

            <Link to="create" className="btn btn-success fs-5">Додати <i className="fa fa-plus-square-o fs-6" aria-hidden="true"></i></Link>
            <table className="table">
                <thead>
                    <tr className="text-center">
                        <th scope="col" >#</th>
                        <th scope="col" style={{ width: '30%' }}>Назва</th>
                        <th scope="col" style={{ width: '30%' }}>Виробник</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {tableViewList}
                </tbody>
            </table>

            <nav aria-label="..." style={{ display: 'flex', justifyContent: 'end' }}>
                <ul className="pagination">
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == 1 }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) - 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) - 1 })} className="fa fa-arrow-left text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                    {pagination}
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == buttons.length }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) + 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) + 1 })} className="fa fa-arrow-right text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                </ul>
            </nav>
        </>
    )
}

export default ProductListPage;